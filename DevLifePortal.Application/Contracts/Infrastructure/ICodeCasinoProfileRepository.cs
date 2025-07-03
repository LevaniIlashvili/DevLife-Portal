using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Infrastructure
{
    public interface ICodeCasinoProfileRepository
    {
        Task CreateProfile(int userId);
        Task<CodeCasinoProfile> GetProfile(int userId);
        Task UpdateProfile(CodeCasinoProfile profile);
    }
}
