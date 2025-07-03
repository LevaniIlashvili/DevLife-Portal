using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Application.DTOs;
using DevLifePortal.Domain.Entities;
using Newtonsoft.Json;

namespace DevLifePortal.Application.Services
{
    public class CodeCasinoService : ICodeCasinoService
    {
        private readonly ICodeCasinoProfileRepository _codeCasinoProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOpenAiService _openAiService;

        public CodeCasinoService(
            ICodeCasinoProfileRepository codeCasinoProfileRepository,
            IUserRepository userRepository,
            IOpenAiService openAiService)
        {
            _codeCasinoProfileRepository = codeCasinoProfileRepository;
            _userRepository = userRepository;
            _openAiService = openAiService;
        }

        public async Task CreateProfile(int userId)
        {
            await _codeCasinoProfileRepository.CreateProfile(userId);
        }

        public async Task<CodeCasinoProfileDTO> GetProfile(int userId)
        {
            var profile = await _codeCasinoProfileRepository.GetProfile(userId);

            var profileDTO = new CodeCasinoProfileDTO()
            {
                Id = profile.Id,
                UserId = profile.UserId,
                Points = profile.Points,
            };
            return profileDTO;
        }

        public async Task<CodeCasinoChallenge> GetSnippets(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            var response = await _openAiService.AskAsync(@$"Give me two similar {user.TechStack} code snippets: one correct and one incorrect. The format should be an object with two properties: correctCode and incorrectCode in JSON format. Do not write anything else, do not include markdown code block markers");

            var snippetsResponse = JsonConvert.DeserializeObject<CodeCasinoChallenge>(response);

            snippetsResponse.TechStack = user.TechStack;

            return snippetsResponse;
        }

        public async Task AnswerChallenge(int userId, bool choseCorrect, int pointsWagered)
        {
            var profile = await _codeCasinoProfileRepository.GetProfile(userId);
            
            if (profile.Points < pointsWagered)
            {
                throw new Exceptions.BadRequestException("You don't have enough points");
            }

            if (choseCorrect)
            {
                profile.Points += pointsWagered * 2;
            } else
            {
                profile.Points -= pointsWagered;
            }

            await _codeCasinoProfileRepository.UpdateProfile(profile);
        }
    }
}
