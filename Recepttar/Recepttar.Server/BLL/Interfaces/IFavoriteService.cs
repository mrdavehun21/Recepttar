using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.Common;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<RecipeCardDto>> GetUserFavoritesAsync(int userId);
        Task<Result> AddFavoriteAsync(CreateFavoriteRecipeDto favoriteRecipeDto);
        Task<Result> RemoveFavoriteAsync(int userId, int recipeId);
    }
}
