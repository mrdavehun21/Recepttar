using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Interfaces
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<IEnumerable<Recipe>> GetByUserIdAsync(int userId);
        Task<Recipe?> GetByIdAsync(int recipeId);
        Task<Recipe> AddAsync(Recipe recipe);
        Task UpdateAsync(Recipe recipe);
        Task DeleteAsync(Recipe recipe);
        Task ReplaceIngredientsAsync(int recipeId, IEnumerable<RecipeIngredient> ingredients);
        Task ReplaceStepsAsync(int recipeId, IEnumerable<RecipeStep> steps);
        Task<IEnumerable<Recipe>> SearchAsync(SearchQueryDto queryDto);
    }
}
