using AutoMapper;
using Recepttar.Server.BLL.Common;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.User;
using Recepttar.Server.BLL.HelperMethods;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto?> RegisterUserAsync(RegisterUserDto registerDto)
        {
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
            {
                return null;
            }

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = PasswordHash.PasswordHasher(registerDto.Password);

            return await _userRepository.CreateAsync(user);
        }

        public async Task<UserDto?> LoginUserAsync(LogInUserDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return null;
            }

            if (user.PasswordHash != PasswordHash.PasswordHasher(loginDto.Password))
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<ResultT<byte[]>> GetUserProfilePictureAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return ResultT<byte[]>.Failure(Messages.Auth.UserNotFound);
            }

            if (user.ProfilePicture == null)
            {
                return ResultT<byte[]>.Failure(Messages.Auth.NoPicture);
            }

            return ResultT<byte[]>.Success(user.ProfilePicture, null);
        }

        public async Task<ProfileDto?> GetProfileByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? null : _mapper.Map<ProfileDto>(user);
        }
        public async Task<ResultT<UpdateResult>> UpdateUserProfileAsync(int userId, UpdateProfileDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ResultT<UpdateResult>.Failure(Messages.Auth.UserNotFound);
            }

            bool wasUpdated = false;

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                user.FullName = updateDto.Name;
                wasUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Email))
            {
                if (await _userRepository.EmailExistsAsync(updateDto.Email, excludeUserId: userId))
                {
                    return ResultT<UpdateResult>.Failure(Messages.Auth.EmailExists);
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

            if (wasUpdated)
            {
                await _userRepository.UpdateAsync(user);
                return ResultT<UpdateResult>.Success(new UpdateResult { WasUpdated = true }, Messages.Auth.Updated);
            }

            return ResultT<UpdateResult>.Success(new UpdateResult { WasUpdated = false }, Messages.Auth.NoChanges);
        }
    }
}
