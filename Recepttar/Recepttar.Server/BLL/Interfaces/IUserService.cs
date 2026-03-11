using Recepttar.Server.BLL.Common;
using Recepttar.Server.BLL.DTOs.User;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> RegisterUserAsync(RegisterUserDto registerDto);
        Task<UserDto?> LoginUserAsync(LogInUserDto loginDto);
        Task<bool> EmailExistsAsync(string email);
        Task<ResultT<byte[]>> GetUserProfilePictureAsync(int userId);
        Task<ProfileDto?> GetProfileByIdAsync(int userId);
        Task<ResultT<UpdateResult>> UpdateUserProfileAsync(int userId, UpdateProfileDto updateDto);
    }
}
