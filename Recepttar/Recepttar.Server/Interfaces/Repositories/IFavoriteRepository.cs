using Recepttar.Server.DTOs.Recipe;

namespace Recepttar.Server.Interfaces.Repositories
{
    public interface IFavoriteRepository
    {
        Task<List<RecipeCardDto>> GetUserFavoritesAsync(int userId);
        Task<(bool success, string message)> AddFavoriteAsync(CreateFavoriteRecipeDto favoriteRecipeDto);
        Task<(bool success, string message)> RemoveFavoriteAsync(int userId, int recipeId);
    }
}
