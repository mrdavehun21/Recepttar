using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.Services;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;
using AutoMapper;
using Moq;

namespace Recepttar.Server.Tests
{
    public class FavoriteServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IFavoriteRepository> _favoriteRepositoryMock;
        private FavoriteService _favoriteService;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(m => m.Map<List<RecipeCardDto>>(It.IsAny<List<Recipe>>()))
                .Returns((List<Recipe> src) =>
                    src.Select(r => new RecipeCardDto { Title = r.Title }).ToList());

            _favoriteRepositoryMock = new Mock<IFavoriteRepository>(MockBehavior.Strict);
            _favoriteService = new FavoriteService(_favoriteRepositoryMock.Object, _mapperMock.Object);
        }

        #region Add Favorite

        [Test]
        public async Task AddFavoriteAsync_ShouldReturnFailure_WhenRecipeNotFound()
        {
            _favoriteRepositoryMock.Setup(r => r.RecipeExistsAsync(99)).ReturnsAsync(false);

            var result = await _favoriteService.AddFavoriteAsync(new CreateFavoriteRecipeDto { RecipeId = 99 });

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Recipe.NotFound));
        }

        [Test]
        public async Task AddFavoriteAsync_ShouldReturnFailure_WhenAlreadyInFavorites()
        {
            _favoriteRepositoryMock.Setup(r => r.RecipeExistsAsync(1)).ReturnsAsync(true);
            _favoriteRepositoryMock.Setup(r => r.GetFavoriteAsync(1, 1)).ReturnsAsync(new Favorite());

            var result = await _favoriteService.AddFavoriteAsync(new CreateFavoriteRecipeDto { UserId = 1, RecipeId = 1 });

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Recipe.AlreadyInFavorites));
        }

        #endregion

        #region Remove Favorite

        [Test]
        public async Task RemoveFavoriteAsync_ShouldReturnFailure_WhenFavoriteNotFound()
        {
            _favoriteRepositoryMock.Setup(r => r.GetFavoriteAsync(1, 99)).ReturnsAsync((Favorite)null);

            var result = await _favoriteService.RemoveFavoriteAsync(1, 99);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Recipe.NotInFavorites));
        }

        #endregion
    }
}
