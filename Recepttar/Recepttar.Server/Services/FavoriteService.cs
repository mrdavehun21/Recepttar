using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Interfaces.Repositories;
using Recepttar.Server.Interfaces.Services;

namespace Recepttar.Server.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<List<RecipeCardDto>> GetUserFavoritesAsync(int userId)
        {
            return await _favoriteRepository.GetUserFavoritesAsync(userId);
        }

        public async Task<(bool success, string message)> AddFavoriteAsync(CreateFavoriteRecipeDto favoriteRecipeDto)
        {
            return await _favoriteRepository.AddFavoriteAsync(favoriteRecipeDto);
        }

        public async Task<(bool success, string message)> RemoveFavoriteAsync(int userId, int recipeId)
        {
            return await _favoriteRepository.RemoveFavoriteAsync(userId, recipeId);
        }
    }
}
