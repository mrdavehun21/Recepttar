using Microsoft.AspNetCore.Http;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.Services;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;
using AutoMapper;
using Moq;

namespace Recepttar.Server.Tests
{
    public class RecipeServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IRecipeRepository> _recipeRepositoryMock;
        private RecipeService _recipeService;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();

            _mapperMock.Setup(m => m.Map<IEnumerable<RecipeCardDto>>(It.IsAny<IEnumerable<Recipe>>()))
                .Returns((IEnumerable<Recipe> src) =>
                    src.Select(r => new RecipeCardDto { Title = r.Title }).ToList());

            _mapperMock.Setup(m => m.Map<RecipeDto>(It.IsAny<Recipe>()))
                .Returns((Recipe src) => new RecipeDto { Title = src.Title });

            _recipeRepositoryMock = new Mock<IRecipeRepository>(MockBehavior.Strict);
            _recipeService = new RecipeService(_recipeRepositoryMock.Object, _mapperMock.Object);
        }

        #region Get by User ID

        [Test]
        public async Task GetRecipesByUserIdAsync_ShouldReturnRecipes_WhenIdIsValid()
        {
            _recipeRepositoryMock.Setup(r => r.GetByUserIdAsync(1))
                .ReturnsAsync(new List<Recipe> { new Recipe { Title = "Pizza" }, new Recipe { Title = "Lobster" } });

            var result = await _recipeService.GetRecipesByUserIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetRecipesByUserIdAsync_ShouldNotReturnRecipes_WhenIdIsInvalid()
        {
            _recipeRepositoryMock.Setup(r => r.GetByUserIdAsync(99))
                .ReturnsAsync(new List<Recipe>());

            var result = await _recipeService.GetRecipesByUserIdAsync(99);

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        #endregion

        #region Get by Recipe ID

        [Test]
        public async Task GetRecipeByIdAsync_shouldReturnRecipe_WhenIdIsvalid()
        {
            _recipeRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Recipe { Title = "Pizza" });

            var result = await _recipeService.GetRecipeByIdAsync(1);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data.Title, Is.EqualTo("Pizza"));
        }

        [Test]
        public async Task GetRecipeByIdAsync_shouldNotReturnRecipe_WhenIdIsInvalid()
        {
            _recipeRepositoryMock.Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Recipe)null);

            var result = await _recipeService.GetRecipeByIdAsync(99);

            Assert.That(result.IsSuccess, Is.False);
        }

        #endregion

        #region Dishpicture

        [Test]
        public async Task GetRecipeImageAsync_shouldReturnSuccess_WhenRecipeHasPicture()
        {
            var fakeImage = new byte[] { 1, 2, 3 };

            _recipeRepositoryMock.Setup(p => p.GetByIdAsync(1))
                .ReturnsAsync(new Recipe { DishPicture = fakeImage });

            var result = await _recipeService.GetRecipeImageAsync(1);

            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public async Task GetRecipeImageAsync_shouldReturnFailure_WhenRecipeIdIsInvalid()
        {
            _recipeRepositoryMock.Setup(p => p.GetByIdAsync(99))
                .ReturnsAsync((Recipe)null);

            var result = await _recipeService.GetRecipeImageAsync(99);

            Assert.That(result.IsSuccess, Is.False);
        }

        #endregion

        #region Add Recipe

        [Test]
        public async Task AddRecipeAsync_ShouldReturnFailure_WhenTimeIsInvalid()
        {
            var createDto = new CreateRecipeDto { TimeMinutes = 0, Servings = 2 };

            var result = await _recipeService.AddRecipeAsync(1, createDto);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Recipe.InvalidTime));
        }

        [Test]
        public async Task AddRecipeAsync_ShouldReturnFailure_WhenNoIngredients()
        {
            var createDto = new CreateRecipeDto
            {
                TimeMinutes = 30,
                Servings = 2,
                Ingredients = new List<IngredientDto>()
            };

            var result = await _recipeService.AddRecipeAsync(1, createDto);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Recipe.NoIngredients));
        }

        [Test]
        public async Task AddRecipeAsync_ShouldReturnSuccess_WhenDtoIsValid()
        {
            var fakePicture = new Mock<IFormFile>();

            var createDto = new CreateRecipeDto
            {
                TimeMinutes = 30,
                Servings = 2,
                Ingredients = new List<IngredientDto> { new IngredientDto() },
                Steps = new List<StepDto> { new StepDto() },
                DishPicture = fakePicture.Object
            };

            var mappedRecipe = new Recipe();
            var createdRecipe = new Recipe { Title = "Pizza" };

            _mapperMock.Setup(m => m.Map<Recipe>(createDto)).Returns(mappedRecipe);

            _recipeRepositoryMock.Setup(r => r.AddAsync(mappedRecipe)).ReturnsAsync(createdRecipe);

            var result = await _recipeService.AddRecipeAsync(1, createDto);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data.Title, Is.EqualTo("Pizza"));
        }

        #endregion

        #region Update Recipe

        [Test]
        public async Task UpdateRecipeAsync_ShouldReturnFailure_WhenRecipeNotFound()
        {
            _recipeRepositoryMock.Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Recipe)null);

            var result = await _recipeService.UpdateRecipeAsync(99, 1, new UpdateRecipeDto());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Recipe.NotFound));
        }

        [Test]
        public async Task UpdateRecipeAsync_ShouldReturnFailure_WhenUserIsNotOwner()
        {
            _recipeRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Recipe { AuthorId = 5 });

            var result = await _recipeService.UpdateRecipeAsync(1, 99, new UpdateRecipeDto());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Recipe.NotOwner));
        }

        #endregion

        #region Remove Recipe

        [Test]
        public async Task RemoveRecipeByIdAsync_ShouldReturnFailure_WhenRecipeNotFound()
        {
            _recipeRepositoryMock.Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Recipe)null);

            var result = await _recipeService.RemoveRecipeByIdAsync(1, 99);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Recipe.NotFound));
        }

        [Test]
        public async Task RemoveRecipeByIdAsync_ShouldReturnFailure_WhenUserIsNotOwner()
        {
            _recipeRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Recipe { AuthorId = 5 });

            var result = await _recipeService.RemoveRecipeByIdAsync(99, 1);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(Messages.Recipe.NotOwner));
        }

        #endregion

        #region Search Recipe

        [Test]
        public async Task SearchRecipesAsync_ShouldReturnRecipes_WhenResultsFound()
        {
            var queryDto = new SearchQueryDto { Search = "Pizza" };

            _recipeRepositoryMock.Setup(r => r.SearchAsync(queryDto))
                .ReturnsAsync(new List<Recipe> { new Recipe { Title = "Pizza" } });

            var result = await _recipeService.SearchRecipesAsync(queryDto);
            
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result, Has.One.Matches<RecipeCardDto>(r => r.Title == "Pizza"));
        }

        [Test]
        public async Task SearchRecipesAsync_ShouldReturnEmpty_WhenNoResultsFound()
        {
            var queryDto = new SearchQueryDto { Search = "Unknown" };

            _recipeRepositoryMock.Setup(r => r.SearchAsync(queryDto))
                .ReturnsAsync(new List<Recipe>());

            var result = await _recipeService.SearchRecipesAsync(queryDto);

            Assert.That(result, Is.Empty);
        }

        #endregion
    }
}
