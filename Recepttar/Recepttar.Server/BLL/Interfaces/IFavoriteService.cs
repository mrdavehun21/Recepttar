using Recepttar.Server.BLL.DTOs.Recipe;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<RecipeCardDto>> GetUserFavoritesAsync(int userId);
        Task<(bool success, string message)> AddFavoriteAsync(CreateFavoriteRecipeDto favoriteRecipeDto);
        Task<(bool success, string message)> RemoveFavoriteAsync(int userId, int recipeId);
    }
}
