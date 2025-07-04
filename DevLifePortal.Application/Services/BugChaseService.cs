using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Services
{
    public class BugChaseService : IBugChaseService
    {
        private readonly IBugChaseProfileRepository _profileRepository;

        public BugChaseService(IBugChaseProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<BugChaseProfile> GetProfile(int userId)
        {
            var profile = await _profileRepository.GetProfile(userId);
            return profile;
        }

        public async Task<List<BugChaseProfile>> GetTopProfiles()
        {
            var profiles = await _profileRepository.GetTopProfiles();
            return profiles;
        }

        public async Task UpdateProfileScore(int userId, int score)
        {
            var profile = await GetProfile(userId);

            if (score > profile.MaxScore)
            {
                profile.MaxScore = score;
                await _profileRepository.UpdateProfile(profile);
            }
        }
    }
}
