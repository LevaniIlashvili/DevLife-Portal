using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface IBugChaseService
    {
        Task<BugChaseProfile> GetProfile(int userId);
        Task<List<BugChaseProfile>> GetTopProfiles();
        Task UpdateProfileScore(int userId, int score);
    }
}
