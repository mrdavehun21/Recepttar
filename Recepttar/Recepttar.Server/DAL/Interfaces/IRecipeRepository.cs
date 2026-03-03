using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Interfaces
{
    public interface IRecipeRepository
    {
        Task<List<Recipe>> GetAllAsync();
        Task<List<Recipe>> GetByUserIdAsync(int userId);
        Task<Recipe?> GetByIdAsync(int recipeId);
        Task<Recipe> AddAsync(Recipe recipe);
        Task UpdateAsync(Recipe recipe);
        Task DeleteAsync(Recipe recipe);
        Task ReplaceIngredientsAsync(int recipeId, List<RecipeIngredient> ingredients);
        Task ReplaceStepsAsync(int recipeId, List<RecipeStep> steps);
        Task<List<Recipe>> SearchAsync(SearchQueryDto queryDto);
    }
}
