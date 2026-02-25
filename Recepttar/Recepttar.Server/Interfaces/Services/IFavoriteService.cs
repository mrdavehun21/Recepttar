using Recepttar.Server.DTOs.Recipe;

namespace Recepttar.Server.Interfaces.Services
{
    public interface IFavoriteService
    {
        public Task<List<RecipeCardDto>> GetUserFavoritesAsync(int userId);

        public Task<(bool success, string message)> AddFavoriteAsync(CreateFavoriteRecipeDto favoriteRecipeDto);

        public Task<(bool success, string message)> RemoveFavoriteAsync(int userId, int recipeId);
    }
}
