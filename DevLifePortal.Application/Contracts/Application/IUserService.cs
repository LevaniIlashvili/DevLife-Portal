using DevLifePortal.Application.DTOs;
using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByUsernameAsync(string username);
        Task<UserDTO> RegisterUser(User user);
    }
}
