using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.Enums;
using Recepttar.Server.BLL.Services;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;
using AutoMapper;
using Moq;

namespace Recepttar.Server.Tests
{
    public class ReferenceDataServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IReferenceDataRepository> _referenceDataRepositoryMock;
        private ReferenceDataService _referenceDataService;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();

            _referenceDataRepositoryMock = new Mock<IReferenceDataRepository>(MockBehavior.Strict);
            _referenceDataService = new ReferenceDataService(_referenceDataRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task SearchTagsAsync_ShouldReturnIngredients_WhenSearchMatches()
        {
            var ingredients = new List<Ingredient> { new Ingredient { Name = "Tomato" } };

            _referenceDataRepositoryMock
                .Setup(r => r.SearchAsync("Tomato", LanguagesEnum.en))
                .ReturnsAsync(ingredients);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<IngredientSearchDto>>(
                    It.IsAny<object>(),
                    It.IsAny<Action<IMappingOperationOptions<object, IEnumerable<IngredientSearchDto>>>>()
                ))
                .Returns((object src, Action<IMappingOperationOptions<object, IEnumerable<IngredientSearchDto>>> opt) =>
                {
                    var ingredientsList = src as IEnumerable<Ingredient>;

                    return new List<IngredientSearchDto>
                    {
                        new IngredientSearchDto { Name = "Tomato" }
                    };
                });

            var result = await _referenceDataService.SearchTagsAsync("Tomato", LanguagesEnum.en);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result, Has.One.Matches<IngredientSearchDto>(i => i.Name == "Tomato"));
        }

        [Test]
        public void GetUnits_ShouldReturnAllUnits()
        {
            var expected = Enum.GetValues<MeasurementUnitEnum>().Select(u => u.ToString()).ToList();

            var result = _referenceDataService.GetUnits();

            Assert.That(result, Is.EquivalentTo(expected));
        }
    }
}
