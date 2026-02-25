using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Enums;
using Recepttar.Server.Interfaces.Repositories;
using Recepttar.Server.Interfaces.Services;

namespace Recepttar.Server.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<List<IngredientSearchDto>> SearchTagsAsync(string? search)
        {
            return await _ingredientRepository.SearchTagsAsync(search);
        }

        public List<string> GetUnits()
        {
            return Enum.GetValues<MeasurementUnitEnum>().Select(u => u.ToString()).ToList();
        }
    }
}
