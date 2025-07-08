using AutoMapper;
using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Application.DTOs;
using DevLifePortal.Application.Validators;
using DevLifePortal.Domain.Entities;
using FluentValidation;

namespace DevLifePortal.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICodeCasinoProfileRepository _codeCasinoProfileRepository;
        private readonly IBugChaseProfileRepository _bugChaseProfileRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository, 
            ICodeCasinoProfileRepository codeCasinoProfileRepository,
            IBugChaseProfileRepository bugChaseProfileRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _codeCasinoProfileRepository = codeCasinoProfileRepository;
            _bugChaseProfileRepository = bugChaseProfileRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                throw new Exceptions.NotFoundException("User not found");
            }

            var userDTO = _mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exceptions.NotFoundException("User not found");
            }

            var userDTO = _mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public async Task<UserDTO> RegisterUser(RegisterUserDTO newUser)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(newUser.Username);

            if (existingUser != null)
            {
                throw new Exceptions.BadRequestException("User with this username already exists");
            }

            var validator = new RegisterUserDTOValidator();
            var result = await validator.ValidateAsync(newUser);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            newUser.ZodiacSign = GetZodiacSign(newUser.DateOfBirth);

            var user = await _userRepository.AddAsync(_mapper.Map<User>(newUser));

            await _codeCasinoProfileRepository.CreateProfile(user.Id);

            await _bugChaseProfileRepository.CreateProfile(user.Id);

            var userDTO = _mapper.Map<UserDTO>(user);

            return userDTO;
        }

        private static string GetZodiacSign(DateOnly date)
        {
            var day = date.Day;
            var month = date.Month;

            return (month, day) switch
            {
                (1, <= 19) => "Capricorn",
                (1, >= 20) or (2, <= 18) => "Aquarius",
                (2, >= 19) or (3, <= 20) => "Pisces",
                (3, >= 21) or (4, <= 19) => "Aries",
                (4, >= 20) or (5, <= 20) => "Taurus",
                (5, >= 21) or (6, <= 20) => "Gemini",
                (6, >= 21) or (7, <= 22) => "Cancer",
                (7, >= 23) or (8, <= 22) => "Leo",
                (8, >= 23) or (9, <= 22) => "Virgo",
                (9, >= 23) or (10, <= 22) => "Libra",
                (10, >= 23) or (11, <= 21) => "Scorpio",
                (11, >= 22) or (12, <= 21) => "Sagittarius",
                (12, >= 22) or (1, <= 19) => "Capricorn",
                _ => "Unknown"
            };
        }
    }
}
