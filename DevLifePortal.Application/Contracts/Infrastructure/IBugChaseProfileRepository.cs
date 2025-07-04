using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Infrastructure
{
    public interface IBugChaseProfileRepository
    {
        Task<BugChaseProfile> CreateProfile(int userId);
        Task<List<BugChaseProfile>> GetTopProfiles();
        Task UpdateProfile(BugChaseProfile bugChaseProfile);
        Task<BugChaseProfile> GetProfile(int userId);
    }
}
