using Recepttar.Server.DTOs.Recipe;

namespace Recepttar.Server.Interfaces.Repositories
{
    public interface IRecipeRepository
    {
        Task<List<RecipeCardDto>> GetRecipesAsync();
        Task<List<RecipeCardDto>> GetRecipesByUserIdAsync(int userId);
        Task<(RecipeDto? dto, string? error)> GetRecipeByIdAsync(int recipeId);
        Task<(byte[]? picture, string? error)> GetRecipeImageAsync(int recipeId);
        Task<(RecipeDto? dto, string? error)> AddRecipeAsync(int userId, CreateRecipeDto createDto);
        Task<(bool success, bool wasUpdated, string? error)> UpdateRecipeAsync(int recipeId, int userId, UpdateRecipeDto updateDto);
        Task<(bool success, string? error)> RemoveRecipeByIdAsync(int userId, int recipeId);
        Task<List<RecipeCardDto>> SearchRecipesAsync(SearchQueryDto queryDto);
    }
}
