using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Models;

namespace Recepttar.Server.Interfaces.Repositories
{
    public interface IIngredientRepository
    {
        Task<List<IngredientSearchDto>> SearchTagsAsync(string? search);
    }
}
