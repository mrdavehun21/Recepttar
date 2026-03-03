using Recepttar.Server.BLL.DTOs.User;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> RegisterUserAsync(RegisterUserDto registerDto);
        Task<UserDto?> LoginUserAsync(LogInUserDto loginDto);
        Task<bool> EmailExistsAsync(string email);
        Task<(byte[]?, string? error)> GetUserProfilePictureAsync(int userId);
        Task<ProfileDto?> GetProfileByIdAsync(int userId);
        Task<(bool success, bool wasUpdated, string? error)> UpdateUserProfileAsync(int userId, UpdateProfileDto updateDto);
    }
}
