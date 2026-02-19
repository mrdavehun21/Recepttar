using Recepttar.Server.DTOs.Recipe;

namespace Recepttar.Server.Interfaces
{
    public interface IFavoriteService
    {
        public Task<List<FavoriteRecipeDto>> GetUserFavoritesAsync(int userId);

        public Task<(bool success, string message)> AddFavoriteAsync(CreateFavoriteRecipeDto favoriteRecipeDto);

        public Task<(bool success, string message)> RemoveFavoriteAsync(int userId, int recipeId);
    }
}
