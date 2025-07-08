using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Infrastructure
{
    public interface ICodeCasinoChallengeRepository
    {
        Task<List<CodeCasinoChallenge>> GetAllAsync();
    }
}
