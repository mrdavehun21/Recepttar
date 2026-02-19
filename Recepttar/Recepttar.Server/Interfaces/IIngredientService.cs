using Recepttar.Server.Models;

namespace Recepttar.Server.Interfaces
{
    public interface IIngredientService
    {
        public Task<List<Ingredient>> SearchTagsAsync(string? search);

        public List<string> GetUnits();
    }
}
