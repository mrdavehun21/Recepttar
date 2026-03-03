using AutoMapper;
using Recepttar.Server.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Recepttar.Server.DAL.Data;
using Recepttar.Server.BLL.DTOs.User;
using Recepttar.Server.DAL.Interfaces;

namespace Recepttar.Server.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.Id != excludeUserId);
        }

        public async Task<UserDto> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
