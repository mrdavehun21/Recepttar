using Recepttar.Server.DTOs.Recipe;

namespace Recepttar.Server.Interfaces.Services
{
    public interface IIngredientService
    {
        public Task<List<IngredientSearchDto>> SearchTagsAsync(string? search);

        public List<string> GetUnits();
    }
}
