using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Infrastructure
{
    public interface IDevDatingProfileRepository
    {
        Task<DevDatingProfile?> GetAsync(int userId);
        Task<DevDatingProfile> AddAsync(DevDatingProfile devDatingProfile);
    }
}
