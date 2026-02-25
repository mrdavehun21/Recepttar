using Recepttar.Server.DTOs.User;

namespace Recepttar.Server.Interfaces.Services
{
    public interface IUserService
    {
        public Task<UserDto?> RegisterUserAsync(RegisterUserDto registerDto);

        public Task<UserDto?> LoginUserAsync(LogInUserDto loginDto);

        public Task<bool> EmailExistsAsync(string email);

        public Task<(bool success, bool wasUpdated, string? error)> UpdateUserProfileAsync(int userId, UpdateProfileDto updateDto);

        public Task<ProfileDto?> GetUserByIdAsync(int userId);

        public Task<(byte[]?, string? error)> GetUserProfilePictureAsync(int userId);
    }
}
