using Recepttar.Server.DTOs.Recipe;

namespace Recepttar.Server.Interfaces
{
    public interface IRecipeService
    {
        Task<List<RecipeDto>> GetRecipesAsync();
        Task<(RecipeDto? dto, string? error)> GetRecipeByIdAsync(int recipeId);
        Task<List<RecipeDto>> GetMyRecipesAsync(int userId);
        Task<(byte[]? picture, string? error)> GetRecipeImageAsync(int recipeId);
        Task<(RecipeDto? dto, string? error)> AddRecipeAsync(int userId, CreateRecipeDto createDto);
        Task<(bool success, bool wasUpdated, string? error)> UpdateRecipeAsync(int recipeId, int userId, UpdateRecipeDto updateDto);
        Task<(bool success, string? error)> RemoveRecipeByIdAsync(int userId, int recipeId);
        Task<List<RecipeDto>> SearchRecipesAsync(SearchQueryDto queryDto);
    }
}
