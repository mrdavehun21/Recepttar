using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Poll;
using Recepttar.Server.BLL.Enums;
using Recepttar.Server.BLL.Services;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;
using AutoMapper;
using Moq;

namespace Recepttar.Server.Tests
{
    public class PollServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IPollRepository> _pollRepositoryMock;
        private PollService _pollService;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();
            
            _pollRepositoryMock = new Mock<IPollRepository>(MockBehavior.Strict);
            _pollService = new PollService(_pollRepositoryMock.Object, _mapperMock.Object);
        }

        #region Create Poll

        [Test]
        public async Task CreatePollAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            _pollRepositoryMock.Setup(r => r.GetUserByIdAsync(99)).ReturnsAsync((User)null);

            var result = await _pollService.CreatePollAsync(99, new PollDto());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Auth.UserNotFound));
        }

        [Test]
        public async Task CreatePollAsync_ShouldReturnFailure_WhenUserRankIsTooLow()
        {
            _pollRepositoryMock.Setup(r => r.GetUserByIdAsync(1))
                .ReturnsAsync(new User { Rank = UserRanksEnum.HomeCook });

            var result = await _pollService.CreatePollAsync(1, new PollDto());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Poll.LowRank));
        }

        [Test]
        public async Task CreatePollAsync_ShouldReturnFailure_WhenQuestionIsEmpty()
        {
            _pollRepositoryMock.Setup(r => r.GetUserByIdAsync(1))
                .ReturnsAsync(new User { Rank = UserRanksEnum.FoodLegend });

            var result = await _pollService.CreatePollAsync(1, new PollDto { Question = " " });

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Poll.NoQuestion));
        }

        [Test]
        public async Task CreatePollAsync_ShouldReturnFailure_WhenTooFewOptions()
        {
            _pollRepositoryMock.Setup(r => r.GetUserByIdAsync(1))
                .ReturnsAsync(new User { Rank = UserRanksEnum.FoodLegend });

            var result = await _pollService.CreatePollAsync(1, new PollDto
            {
                Question = "Best dish?",
                Options = new List<PollOptionDto>()
            });

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Poll.LowOptions));
        }

        #endregion

        #region Add Vote

        [Test]
        public async Task AddVoteAsync_ShouldReturnFailure_WhenPollNotFound()
        {
            _pollRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Poll)null);

            var result = await _pollService.AddVoteAsync(1, 99, 1);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Poll.NotFound));
        }

        [Test]
        public async Task AddVoteAsync_ShouldReturnFailure_WhenOptionDoesNotBelongToPoll()
        {
            _pollRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Poll());
            _pollRepositoryMock.Setup(r => r.OptionBelongsToPollAsync(1, 99)).ReturnsAsync(false);

            var result = await _pollService.AddVoteAsync(1, 1, 99);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Poll.InvalidOption));
        }

        [Test]
        public async Task AddVoteAsync_ShouldReturnFailure_WhenUserAlreadyVoted()
        {
            _pollRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Poll());
            _pollRepositoryMock.Setup(r => r.OptionBelongsToPollAsync(1, 1)).ReturnsAsync(true);
            _pollRepositoryMock.Setup(r => r.GetExistingVoteAsync(1, 1)).ReturnsAsync(new Vote());

            var result = await _pollService.AddVoteAsync(1, 1, 1);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Poll.Voted));
        }

        #endregion

        #region Delete Poll

        [Test]
        public async Task DeletePollAsync_ShouldReturnFailure_WhenPollNotFound()
        {
            _pollRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Poll)null);

            var result = await _pollService.DeletePollAsync(1, 99);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Poll.NotFound));
        }

        [Test]
        public async Task DeletePollAsync_ShouldReturnFailure_WhenUserIsNotOwner()
        {
            _pollRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Poll { AuthorId = 5 });

            var result = await _pollService.DeletePollAsync(99, 1);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Poll.NotOwnerDelete));
        }

        #endregion

        #region Update Poll

        [Test]
        public async Task UpdatePollAsync_ShouldReturnFailure_WhenPollNotFound()
        {
            _pollRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Poll)null);

            var result = await _pollService.UpdatePollAsync(1, 99, new PollDto());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Poll.NotFound));
        }

        [Test]
        public async Task UpdatePollAsync_ShouldReturnFailure_WhenUserIsNotOwner()
        {
            _pollRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Poll { AuthorId = 5 });

            var result = await _pollService.UpdatePollAsync(99, 1, new PollDto());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Poll.NotOwner));
        }

        #endregion
    }
}
