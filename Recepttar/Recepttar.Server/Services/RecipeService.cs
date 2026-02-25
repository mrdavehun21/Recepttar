using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Interfaces.Repositories;
using Recepttar.Server.Interfaces.Services;

namespace Recepttar.Server.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<List<RecipeCardDto>> GetRecipesAsync()
        {
            return await _recipeRepository.GetRecipesAsync();
        }

        public async Task<List<RecipeCardDto>> GetRecipesByIdAsync(int userId)
        {
            return await _recipeRepository.GetRecipesByUserIdAsync(userId);
        }

        public async Task<(RecipeDto? dto, string? error)> GetRecipeByIdAsync(int recipeId)
        {
            return await _recipeRepository.GetRecipeByIdAsync(recipeId);
        }

        public async Task<(byte[]? picture, string? error)> GetRecipeImageAsync(int recipeId)
        {
            return await _recipeRepository.GetRecipeImageAsync(recipeId);
        }

        public async Task<(RecipeDto? dto, string? error)> AddRecipeAsync(int userId, CreateRecipeDto createDto)
        {
            return await _recipeRepository.AddRecipeAsync(userId, createDto);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdateRecipeAsync(int recipeId, int userId, UpdateRecipeDto updateDto)
        {
            return await _recipeRepository.UpdateRecipeAsync(recipeId, userId, updateDto);
        }

        public async Task<(bool success, string? error)> RemoveRecipeByIdAsync(int userId, int recipeId)
        {
            return await _recipeRepository.RemoveRecipeByIdAsync(userId, recipeId);
        }

        public async Task<List<RecipeCardDto>> SearchRecipesAsync(SearchQueryDto queryDto)
        {
            return await _recipeRepository.SearchRecipesAsync(queryDto);
        }
    }
}
