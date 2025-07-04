using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface IDevDatingService
    {
        Task<DevDatingProfile> CreateProfileAsync(DevDatingProfile profile);
        Task<DevDatingProfile> GetProfileAsync(int userId);
        Task<DevDatingFakeProfile?> GetPotentialMatch(int userId);
        Task SwipeAsync(DevDatingSwipeAction swipeAction);
        Task<List<DevDatingFakeProfile>> GetMatchesAsync(int userId);
        Task<string> ChatWithFakeProfileAi(int userId, Guid fakeProfileId, string userText);
    }
}
