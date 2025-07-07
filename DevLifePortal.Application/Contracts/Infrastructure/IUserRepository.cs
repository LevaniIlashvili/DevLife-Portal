using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Infrastructure
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
    }
}
