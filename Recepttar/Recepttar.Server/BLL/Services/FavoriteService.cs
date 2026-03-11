using AutoMapper;
using Recepttar.Server.BLL.Common;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;

        public FavoriteService(IFavoriteRepository favoriteRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RecipeCardDto>> GetUserFavoritesAsync(int userId)
        {
            var favorites = await _favoriteRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RecipeCardDto>>(favorites.Select(f => f.Recipe));
        }

        public async Task<Result> AddFavoriteAsync(CreateFavoriteRecipeDto favoriteRecipeDto)
        {
            if (!await _favoriteRepository.RecipeExistsAsync(favoriteRecipeDto.RecipeId))
            {
                return Result.Failure(Messages.Recipe.NotFound);
            }

            if (await _favoriteRepository.GetFavoriteAsync(favoriteRecipeDto.UserId, favoriteRecipeDto.RecipeId) != null)
            {
                return Result.Failure(Messages.Recipe.AlreadyInFavorites);
            }

            await _favoriteRepository.AddFavoriteAsync(new Favorite
            {
                UserId = favoriteRecipeDto.UserId,
                RecipeId = favoriteRecipeDto.RecipeId
            });

            return Result.Success(Messages.Recipe.AddToFavorites);
        }

        public async Task<Result> RemoveFavoriteAsync(int userId, int recipeId)
        {
            var favorite = await _favoriteRepository.GetFavoriteAsync(userId, recipeId);
            if (favorite == null)
            {
                return Result.Failure(Messages.Recipe.NotInFavorites);
            }

            await _favoriteRepository.RemoveFavoriteAsync(favorite);
            return Result.Success();
        }
    }
}
