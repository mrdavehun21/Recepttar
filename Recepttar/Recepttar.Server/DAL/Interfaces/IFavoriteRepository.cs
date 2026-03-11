using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId);
        Task<bool> RecipeExistsAsync(int recipeId);
        Task<Favorite?> GetFavoriteAsync(int userId, int recipeId);
        Task AddFavoriteAsync(Favorite favorite);
        Task RemoveFavoriteAsync(Favorite favorite);
    }
}
