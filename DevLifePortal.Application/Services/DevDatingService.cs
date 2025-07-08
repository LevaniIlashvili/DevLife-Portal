using AutoMapper;
using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Application.DTOs;
using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Services
{
    public class DevDatingService : IDevDatingService
    {
        private readonly IDevDatingProfileRepository _profileRepository;
        private readonly IDevDatingFakeProfileRepository _fakeProfileRepository;
        private readonly IDevDateSwipeRepository _swipeRepository;
        private readonly IOpenAiService _openAiService;
        private readonly IMapper _mapper;

        public DevDatingService(
            IDevDatingProfileRepository profileRepository, 
            IDevDatingFakeProfileRepository fakeProfileRepository,
            IDevDateSwipeRepository swipeRepository,
            IOpenAiService openAiService,
            IMapper mapper)
        {
            _profileRepository = profileRepository;
            _fakeProfileRepository = fakeProfileRepository;
            _swipeRepository = swipeRepository;
            _openAiService = openAiService;
            _mapper = mapper;
        }

        public async Task<DevDatingProfileDTO> CreateProfileAsync(DevDatingAddProfileDTO profile, int userId)
        {
            var profileToAdd = _mapper.Map<DevDatingProfile>(profile);
            profileToAdd.UserId = userId;
            var addedProfile = await _profileRepository.AddAsync(profileToAdd);

            return _mapper.Map<DevDatingProfileDTO>(addedProfile);
        }

        public async Task<DevDatingProfileDTO> GetProfileAsync(int userId)
        {
            var profile = await _profileRepository.GetAsync(userId);

            if (profile == null)
            {
                throw new Exceptions.NotFoundException("Profile doesn't exist");
            }

            return _mapper.Map<DevDatingProfileDTO>(profile);
        }

        public async Task<DevDatingFakeProfile?> GetPotentialMatch(int userId)
        {
            var userProfile = await _profileRepository.GetAsync(userId);
            var allFakeProfiles = await _fakeProfileRepository.GetAllAsync();

            var swipedIds = await _swipeRepository.GetSwipedFakeProfileIdsAsync(userId);

            var userPreference = userProfile.PrefersMale ? "Male" : "Female";
            var userGender = userProfile.IsMale ? "Male" : "Female";

            var compatibleProfiles = allFakeProfiles
                .Where(p =>
                    p.Gender == userPreference &&
                    p.Preference == userGender &&
                    !swipedIds.Contains(p.Id)
                )
                .ToList();

            if (!compatibleProfiles.Any())
                return null;

            var random = new Random();
            int index = random.Next(compatibleProfiles.Count);
            return compatibleProfiles[index];
        }

        public async Task SwipeAsync(DevDatingSwipeAction swipeAction)
        {
            await _swipeRepository.SaveSwipeAsync(swipeAction);
        }

        public async Task<List<DevDatingFakeProfile>> GetMatchesAsync(int userId)
        {
            var swipedRightIds = await _swipeRepository.GetRightSwipedFakeProfileIdsAsync(userId);
            var allFakeProfiles = await _fakeProfileRepository.GetAllAsync();

            return allFakeProfiles
                .Where(p => swipedRightIds.Contains(p.Id))
                .ToList();
        }

        public async Task<string> ChatWithFakeProfileAi(int userId, Guid fakeProfileId, string userText)
        {
            var userProfile = await _profileRepository.GetAsync(userId);
            var fakeProfile = await _fakeProfileRepository.GetByIdAsync(fakeProfileId);

            if (fakeProfile == null)
            {
                throw new Exceptions.NotFoundException("Fake profile not found");
            }

            var userGender = userProfile.IsMale ? "male" : "female";

            var response = await _openAiService.AskAsync(
                @$"lets play out fake scenario, we are on online dev dating website and we matched,
                   im {userGender}, my bio is {userProfile.Bio}, you are {fakeProfile.Gender},
                   your bio is {fakeProfile.Bio}, your teck stack is {fakeProfile.TechStack}
                   and i texted you this {userText}");

            return response;
        }

    }
}
