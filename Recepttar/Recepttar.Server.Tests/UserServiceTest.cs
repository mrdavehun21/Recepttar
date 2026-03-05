using Recepttar.Server.BLL.DTOs.User;
using Recepttar.Server.BLL.Services;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;
using Recepttar.Server.BLL.HelperMethods;
using AutoMapper;
using Moq;

namespace Recepttar.Server.Tests
{
    public class UserServiceTest
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();
            
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns(new UserDto { Email = "user@test.com" });

            _mapperMock.Setup(m => m.Map<ProfileDto>(It.IsAny<User>()))
                .Returns(new ProfileDto { FullName = "James Randal" });

            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        #region Register

        [Test]
        public async Task RegisterUserAsync_ShouldReturnNull_WhenEmailAlreadyExists()
        {
            _userRepositoryMock.Setup(r => r.EmailExistsAsync("taken@test.com", null))
                     .ReturnsAsync(true);

            var result = await _userService.RegisterUserAsync(new RegisterUserDto
            {
                FullName = "James Randal",
                Email = "taken@test.com",
                Password = "password123"
            });

            Assert.That(result, Is.Null);
            _userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        #endregion

        #region Login

        [Test]
        public async Task LoginUserAsync_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var passwordHash = PasswordHash.PasswordHasher("correctpassword");

            _userRepositoryMock.Setup(r => r.GetByEmailAsync("user@test.com"))
                .ReturnsAsync(new User
                {
                    Email = "user@test.com",
                    PasswordHash = passwordHash
                });

            var result = await _userService.LoginUserAsync(new LogInUserDto
            {
                Email = "user@test.com",
                Password = "correctpassword"
            });

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("user@test.com"));
        }
        
        [Test]
        public async Task EmailExistsAsync_ShouldReturnTrue_WhenEmailIsValid()
        {
            _userRepositoryMock.Setup(r => r.EmailExistsAsync("user@test.com", null))
                     .ReturnsAsync(true);

            var result = await _userService.EmailExistsAsync("user@test.com");

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task EmailExistsAsync_ShouldReturnFalse_WhenEmailIsInvalid()
        {
            _userRepositoryMock.Setup(r => r.EmailExistsAsync("user@test.com", null))
                     .ReturnsAsync(true);

            var result = await _userService.EmailExistsAsync("invalid@test.com");

            Assert.That(result, Is.False);
        }

        #endregion

        #region Profile picture

        [Test]
        public async Task GetUserProfilePictureAsync_ShouldReturnSuccess_WhenUserHasPicture()
        {
            var fakeImage = new byte[] { 1, 2, 3 };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new User { Id = 1, ProfilePicture = fakeImage });

            var result = await _userService.GetUserProfilePictureAsync(1);

            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public async Task GetUserProfilePictureAsync_ShouldReturnFailure_WhenUserHasNoPicture()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new User { Id = 1, ProfilePicture = null });

            var result = await _userService.GetUserProfilePictureAsync(1);

            Assert.That(result.IsSuccess, Is.False);
        }

        #endregion

        #region Profile

        [Test]
        public async Task GetProfileByIdAsync_ShouldReturnUser_WhenIdIsValid()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new User { Id = 1, FullName = "James Randal" });

            var result = await _userService.GetProfileByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FullName, Is.EqualTo("James Randal"));
        }

        [Test]
        public async Task GetProfileByIdAsync_ShouldReturnNull_WhenIdIsInvalid()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((User?)null);

            var result = await _userService.GetProfileByIdAsync(99);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateUserProfileAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((User?)null);

            var result = await _userService.UpdateUserProfileAsync(1, new UpdateProfileDto());

            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task UpdateUserProfileAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new User { Id = 1, Email = "old@test.com" });

            _userRepositoryMock.Setup(r => r.EmailExistsAsync("taken@test.com", 1))
                .ReturnsAsync(true);

            var result = await _userService.UpdateUserProfileAsync(1, new UpdateProfileDto
            {
                Email = "taken@test.com"
            });

            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task UpdateUserProfileAsync_ShouldReturnWasUpdatedTrue_WhenValidChanges()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new User { Id = 1 });

            _userRepositoryMock.Setup(r => r.EmailExistsAsync("new@test.com", 1))
                .ReturnsAsync(false);

            var result = await _userService.UpdateUserProfileAsync(1, new UpdateProfileDto
            {
                Name = "James Randal",
                Email = "new@test.com"
            });

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data.WasUpdated, Is.True);
        }

        [Test]
        public async Task UpdateUserProfileAsync_ShouldReturnWasUpdatedFalse_WhenNothingChanged()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new User { Id = 1 });

            var result = await _userService.UpdateUserProfileAsync(1, new UpdateProfileDto());

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data.WasUpdated, Is.False);
        }

        [Test]
        public async Task UpdateUserProfileAsync_ShouldCallUpdateAsync_WhenChangesAreMade()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new User { Id = 1 });

            await _userService.UpdateUserProfileAsync(1, new UpdateProfileDto { Name = "James Randal" });

            _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        #endregion
    }
}