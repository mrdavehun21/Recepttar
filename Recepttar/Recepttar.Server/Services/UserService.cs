using Recepttar.Server.DTOs.User;
using Recepttar.Server.Interfaces.Repositories;
using Recepttar.Server.Interfaces.Services;

namespace Recepttar.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> RegisterUserAsync(RegisterUserDto registerDto)
        {
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
            {
                return null;
            }

            return await _userRepository.CreateAsync(registerDto);
        }

        public async Task<UserDto?> LoginUserAsync(LogInUserDto loginDto)
        {
            return await _userRepository.ValidateCredentialsAsync(loginDto);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdateUserProfileAsync(int userId, UpdateProfileDto updateDto)
        {
            return await _userRepository.UpdateAsync(userId, updateDto);
        }

        public async Task<ProfileDto?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetProfileByIdAsync(userId);
        }

        public async Task<(byte[]?, string? error)> GetUserProfilePictureAsync(int userId)
        {
            return await _userRepository.GetProfilePictureAsync(userId);
        }
    }
}
