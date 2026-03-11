using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Review;
using Recepttar.Server.BLL.Services;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;
using AutoMapper;
using Moq;

namespace Recepttar.Server.Tests
{
    public class ReviewServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IReviewRepository> _reviewRepositoryMock;
        private ReviewService _reviewService;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();

            _mapperMock.Setup(m => m.Map<List<ReviewDto>>(It.IsAny<IEnumerable<Review>>()))
            .Returns((IEnumerable<Review> src) =>
                src.Select(r => new ReviewDto { Comment = r.Comment }).ToList());

            _mapperMock.Setup(m => m.Map<Review>(It.IsAny<AddReviewDto>()))
                .Returns((AddReviewDto src) => new Review { Stars = src.Stars, Comment = src.Comment });

            _reviewRepositoryMock = new Mock<IReviewRepository>(MockBehavior.Strict);
            _reviewService = new ReviewService(_reviewRepositoryMock.Object, _mapperMock.Object);
        }

        #region Get Reviews

        [Test]
        public async Task GetRecipeReviewsAsync_ShouldReturnNull_WhenRecipeDoesNotExist()
        {
            _reviewRepositoryMock.Setup(r => r.RecipeExistsAsync(99)).ReturnsAsync(false);

            var result = await _reviewService.GetRecipeReviewsAsync(99);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetRecipeReviewsAsync_ShouldReturnReviews_WhenRecipeExists()
        {
            _reviewRepositoryMock.Setup(r => r.RecipeExistsAsync(1)).ReturnsAsync(true);
            _reviewRepositoryMock.Setup(r => r.GetRecipeReviewsAsync(1))
                .ReturnsAsync(new List<Review> { new Review { Comment = "Nice" } });

            var result = await _reviewService.GetRecipeReviewsAsync(1);

            Assert.That(result, Is.Not.Null);
        }

        #endregion

        #region Add Review

        [Test]
        public async Task AddReviewAsync_ShouldReturnFailure_WhenRecipeNotFound()
        {
            _reviewRepositoryMock.Setup(r => r.RecipeExistsAsync(99)).ReturnsAsync(false);

            var result = await _reviewService.AddReviewAsync(1, 99, new AddReviewDto());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Recipe.NotFound));
        }

        [Test]
        public async Task AddReviewAsync_ShouldReturnFailure_WhenUserAlreadyReviewed()
        {
            _reviewRepositoryMock.Setup(r => r.RecipeExistsAsync(1)).ReturnsAsync(true);
            _reviewRepositoryMock.Setup(r => r.ReviewExistsForUserAsync(1, 1)).ReturnsAsync(true);

            var result = await _reviewService.AddReviewAsync(1, 1, new AddReviewDto());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Review.AlreadyReviewed));
        }

        [Test]
        public async Task AddReviewAsync_ShouldReturnFailure_WhenStarsAreInvalid()
        {
            _reviewRepositoryMock.Setup(r => r.RecipeExistsAsync(1)).ReturnsAsync(true);
            _reviewRepositoryMock.Setup(r => r.ReviewExistsForUserAsync(1, 1)).ReturnsAsync(false);

            var result = await _reviewService.AddReviewAsync(1, 1, new AddReviewDto { Stars = 10 });

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Review.InvalidStars));
        }

        #endregion

        #region Update Review

        [Test]
        public async Task UpdateReviewAsync_ShouldReturnFailure_WhenReviewNotFound()
        {
            _reviewRepositoryMock.Setup(r => r.GetReviewByIdAsync(99)).ReturnsAsync((Review)null);

            var result = await _reviewService.UpdateReviewAsync(1, 99, new UpdateReviewDto());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Review.NotFound));
        }

        [Test]
        public async Task UpdateReviewAsync_ShouldReturnFailure_WhenUserIsNotOwner()
        {
            _reviewRepositoryMock.Setup(r => r.GetReviewByIdAsync(1))
                .ReturnsAsync(new Review { UserId = 5 });

            var result = await _reviewService.UpdateReviewAsync(99, 1, new UpdateReviewDto());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Review.NotOwner));
        }

        #endregion

        #region Delete Review

        [Test]
        public async Task DeleteReviewAsync_ShouldReturnFailure_WhenReviewNotFound()
        {
            _reviewRepositoryMock.Setup(r => r.GetReviewByIdAsync(99)).ReturnsAsync((Review)null);

            var result = await _reviewService.DeleteReviewAsync(1, 99);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Review.NotFound));
        }

        [Test]
        public async Task DeleteReviewAsync_ShouldReturnFailure_WhenUserIsNotOwner()
        {
            _reviewRepositoryMock.Setup(r => r.GetReviewByIdAsync(1))
                .ReturnsAsync(new Review { UserId = 5 });

            var result = await _reviewService.DeleteReviewAsync(99, 1);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Review.NotOwnerDelete));
        }

        #endregion
    }
}
