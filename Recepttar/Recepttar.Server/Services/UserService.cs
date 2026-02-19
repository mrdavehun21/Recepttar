using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Constants;
using Recepttar.Server.Data;
using Recepttar.Server.DTOs.User;
using Recepttar.Server.Enums;
using Recepttar.Server.HelperMethods;
using Recepttar.Server.Interfaces;
using Recepttar.Server.Models;

namespace Recepttar.Server.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> RegisterUserAsync(RegisterUserDto registerDto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);

            if (userExists)
            {
                return null;
            }

            string hashedPassword = PasswordHash.PasswordHasher(registerDto.Password);

            var user = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
                Bio = string.Empty,
                ProfilePicture = Array.Empty<byte>(),
                Rank = UserRanksEnum.HomeCook
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Bio = user.Bio,
                Rank = user.Rank
            };
        }

        public async Task<UserDto?> LoginUserAsync(LogInUserDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            string hashedPassword = PasswordHash.PasswordHasher(loginDto.Password);

            if (user == null)
            {
                return null;
            }

            if (user.PasswordHash != hashedPassword)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Bio = user.Bio,
                Rank = user.Rank
            };
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdateUserProfileAsync(int userId, UpdateProfileDto updateDto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return (false, false, Messages.Auth.UserNotFound);
            }

            bool wasUpdated = false;

            // Update Name
            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                user.FullName = updateDto.Name;
                wasUpdated = true;
            }

            // Update Email
            if (!string.IsNullOrWhiteSpace(updateDto.Email))
            {
                var emailExists = await _context.Users.AnyAsync(u => u.Email == updateDto.Email && u.Id != userId);

                if (emailExists)
                {
                    return (false, false, Messages.Auth.EmailExists);
                }

                user.Email = updateDto.Email;
                wasUpdated = true;
            }

            // Update Password
            if (!string.IsNullOrWhiteSpace(updateDto.Password))
            {
                user.PasswordHash = PasswordHash.PasswordHasher(updateDto.Password);
                wasUpdated = true;
            }

            // Update Bio
            if (!string.IsNullOrWhiteSpace(updateDto.Bio))
            {
                user.Bio = updateDto.Bio;
                wasUpdated = true;
            }

            // Update Profile Picture
            if (updateDto.ProfilePicture != null)
            {
                using (var stream = new MemoryStream())
                {
                    await updateDto.ProfilePicture.CopyToAsync(stream);
                    user.ProfilePicture = stream.ToArray();
                    wasUpdated = true;
                }
            }

            if (wasUpdated)
            {
                await _context.SaveChangesAsync();
            }

            return (true, wasUpdated, null);
        }

        public async Task<ProfileDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new ProfileDto
            {
                FullName = user.FullName,
                Bio = user.Bio,
                ProfilePicture = ProfilePicturePath.GetPath(user.Id),
                Rank = user.Rank
            };
        }

        public async Task<(byte[]?, string? error)> GetUserProfilePictureAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return (null, Messages.Auth.UserNotFound);
            }

            if(user.ProfilePicture == null)
            {
                return (null, Messages.Auth.NoPicture);
            }

            return (user.ProfilePicture, null);
        }
    }
}
