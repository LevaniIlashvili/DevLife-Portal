using DevLifePortal.Application.DTOs;
using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface ICodeCasinoService
    {
        Task CreateProfile(int userId);
        Task<CodeCasinoProfileDTO> GetProfile(int userId);
        Task<CodeCasinoChallenge> GetSnippets(string username);
        Task AnswerChallenge(int userId, CodeCasinoAnswerChallengeDTO answerChallengeDTO);

    }
}
