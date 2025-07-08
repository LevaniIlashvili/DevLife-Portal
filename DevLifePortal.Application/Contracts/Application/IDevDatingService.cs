using DevLifePortal.Application.DTOs;
using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface IDevDatingService
    {
        Task<DevDatingProfileDTO> CreateProfileAsync(DevDatingAddProfileDTO profile, int userId);
        Task<DevDatingProfileDTO> GetProfileAsync(int userId);
        Task<DevDatingFakeProfile?> GetPotentialMatch(int userId);
        Task SwipeAsync(DevDatingSwipeAction swipeAction);
        Task<List<DevDatingFakeProfile>> GetMatchesAsync(int userId);
        Task<string> ChatWithFakeProfileAi(DevDatingTextMatchDTO textMatchDTO, int userId);
    }
}
