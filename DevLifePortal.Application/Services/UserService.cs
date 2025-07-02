using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> RegisterUser(User newUser)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(newUser.Username);

            if (existingUser != null)
            {
                throw new Exceptions.BadRequestException("User with this username already exists");
            }

            return await _userRepository.AddAsync(newUser);
        }
    }
}
