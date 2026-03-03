using Recepttar.Server.BLL.DTOs.User;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
        Task<UserDto> CreateAsync(User user);
        Task UpdateAsync(User user);
    }
}
