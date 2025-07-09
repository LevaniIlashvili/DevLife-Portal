using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Application.DTOs;
using DevLifePortal.Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DevLifePortal.Application.Services
{
    public class CodeCasinoService : ICodeCasinoService
    {
        private readonly ICodeCasinoProfileRepository _codeCasinoProfileRepository;
        private readonly ICodeCasinoChallengeRepository _codeCasinoChallengeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOpenAiService _openAiService;
        private readonly ILogger<CodeCasinoService> _logger;

        public CodeCasinoService(
            ICodeCasinoProfileRepository codeCasinoProfileRepository,
            ICodeCasinoChallengeRepository codeCasinoChallengeRepository,
            IUserRepository userRepository,
            IOpenAiService openAiService,
            ILogger<CodeCasinoService> logger)
        {
            _codeCasinoProfileRepository = codeCasinoProfileRepository;
            _codeCasinoChallengeRepository = codeCasinoChallengeRepository;
            _userRepository = userRepository;
            _openAiService = openAiService;
            _logger = logger;
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

            CodeCasinoChallenge snippetsResponse;
            try
            {
                var response = await _openAiService.AskAsync(@$"Give me two similar {user.TechStack} code snippets: one correct and one incorrect. The format should be an object with two properties: correctCode and incorrectCode in JSON format. Do not write anything else, do not include markdown code block markers");

                snippetsResponse = JsonConvert.DeserializeObject<CodeCasinoChallenge>(response);
                snippetsResponse.TechStack = user.TechStack;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting code casino challenge from OpenAI API");
                var snippets = await _codeCasinoChallengeRepository.GetAllAsync();
                var rand = new Random();
                snippetsResponse = snippets[rand.Next(0, snippets.Count)];
            }

            return snippetsResponse;
        }

        public async Task AnswerChallenge(int userId, CodeCasinoAnswerChallengeDTO answerChallengeDTO)
        {
            var profile = await _codeCasinoProfileRepository.GetProfile(userId);
            
            if (profile.Points < answerChallengeDTO.PointsWagered)
            {
                throw new Exceptions.BadRequestException("You don't have enough points");
            }

            if (answerChallengeDTO.ChoseCorrect)
            {
                profile.Points += answerChallengeDTO.PointsWagered * 2;
            } else
            {
                profile.Points -= answerChallengeDTO.PointsWagered;
            }

            await _codeCasinoProfileRepository.UpdateProfile(profile);
        }
    }
}
