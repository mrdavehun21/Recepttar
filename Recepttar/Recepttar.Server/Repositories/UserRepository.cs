using AutoMapper;
using Recepttar.Server.Constants;
using Recepttar.Server.Data;
using Recepttar.Server.DTOs.User;
using Recepttar.Server.HelperMethods;
using Recepttar.Server.Models;
using Recepttar.Server.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Recepttar.Server.Repositories
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

        public async Task<UserDto?> GetByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<ProfileDto?> GetProfileByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            return user == null ? null : _mapper.Map<ProfileDto>(user);
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> ValidateCredentialsAsync(LogInUserDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return null;
            }

            string hashedPassword = PasswordHash.PasswordHasher(loginDto.Password);
            if (user.PasswordHash != hashedPassword)
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.Id != excludeUserId);
        }

        public async Task<(byte[]?, string? error)> GetProfilePictureAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return (null, Messages.Auth.UserNotFound);
            }

            if (user.ProfilePicture == null)
            {
                return (null, Messages.Auth.NoPicture);
            }

            return (user.ProfilePicture, null);
        }

        public async Task<UserDto> CreateAsync(RegisterUserDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdateAsync(int userId, UpdateProfileDto updateDto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return (false, false, Messages.Auth.UserNotFound);
            }

            bool wasUpdated = false;

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                user.FullName = updateDto.Name;
                wasUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Email))
            {
                if (await EmailExistsAsync(updateDto.Email, excludeUserId: userId))
                {
                    return (false, false, Messages.Auth.EmailExists);
                }

                user.Email = updateDto.Email;
                wasUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Password))
            { 
                user.PasswordHash = PasswordHash.PasswordHasher(updateDto.Password); 
                wasUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Bio))
            { 
                user.Bio = updateDto.Bio;
                wasUpdated = true;
            }

            if (updateDto.ProfilePicture != null)
            {
                using var stream = new MemoryStream();
                await updateDto.ProfilePicture.CopyToAsync(stream);
                user.ProfilePicture = stream.ToArray();
                wasUpdated = true;
            }

            if (wasUpdated) await _context.SaveChangesAsync();

            return (true, wasUpdated, null);
        }
    }
}
