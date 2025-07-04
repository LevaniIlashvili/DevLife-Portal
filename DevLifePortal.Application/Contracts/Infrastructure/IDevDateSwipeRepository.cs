using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Infrastructure
{
    public interface IDevDateSwipeRepository
    {
        Task<List<Guid>> GetSwipedFakeProfileIdsAsync(int userId);
        Task SaveSwipeAsync(DevDatingSwipeAction swipe);
        Task<List<Guid>> GetRightSwipedFakeProfileIdsAsync(int userId);
    }
}
