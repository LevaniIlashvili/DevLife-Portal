using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Infrastructure
{
    public interface IDevDatingFakeProfileRepository
    {
        Task<List<DevDatingFakeProfile>> GetAllAsync();
        Task<DevDatingFakeProfile?> GetByIdAsync(Guid id);
    }
}
