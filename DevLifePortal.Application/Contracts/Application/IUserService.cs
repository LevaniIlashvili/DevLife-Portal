using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface IUserService
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> RegisterUser(User user);
    }
}
