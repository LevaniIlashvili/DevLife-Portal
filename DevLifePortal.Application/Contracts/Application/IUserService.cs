using DevLifePortal.Application.DTOs;
using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByUsernameAsync(string username);
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO> RegisterUser(RegisterUserDTO user);
    }
}
