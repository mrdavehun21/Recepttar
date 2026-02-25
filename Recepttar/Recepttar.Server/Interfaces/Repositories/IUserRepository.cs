using Recepttar.Server.DTOs.User;

namespace Recepttar.Server.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto?> GetByIdAsync(int userId);
        Task<ProfileDto?> GetProfileByIdAsync(int userId);
        Task<UserDto?> ValidateCredentialsAsync(LogInUserDto loginDto);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
        Task<(byte[]?, string? error)> GetProfilePictureAsync(int userId);
        Task<UserDto> CreateAsync(RegisterUserDto registerDto);
        Task<(bool success, bool wasUpdated, string? error)> UpdateAsync(int userId, UpdateProfileDto updateDto);
    }
}
