using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Constants;
using Recepttar.Server.Data;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Interfaces;
using Recepttar.Server.Models;

namespace Recepttar.Server.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly AppDbContext _context;

        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FavoriteRecipeDto>> GetUserFavoritesAsync(int userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => new FavoriteRecipeDto
                {
                    RecipeId = f.RecipeId,
                    Title = f.Recipe.Title,
                    Description = f.Recipe.Description,
                    DishPicture = DishPicturePath.GetPath(f.RecipeId)
                }).ToListAsync();
        }

        public async Task<(bool success, string message)> AddFavoriteAsync(CreateFavoriteRecipeDto favoriteRecipeDto)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == favoriteRecipeDto.RecipeId);

            if (recipe == null)
            {
                return (false, Messages.Recipe.NotFound);
            }

            var existingFavorite = await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == favoriteRecipeDto.UserId && f.RecipeId == favoriteRecipeDto.RecipeId);

            if (existingFavorite != null)
            {
                return (false, Messages.Recipe.AlreadyInFavorites);
            }

            await _context.Favorites.AddAsync(new Favorite
            {
                UserId = favoriteRecipeDto.UserId,
                RecipeId = favoriteRecipeDto.RecipeId
            });

            await _context.SaveChangesAsync();

            return (true, Messages.Recipe.AddToFavorites);
        }

        public async Task<(bool success, string message)> RemoveFavoriteAsync(int userId, int recipeId)
        {
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);

            if (favorite == null)
            {
                return (false, Messages.Recipe.NotInFavorites);
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return (true, Messages.Recipe.RemovedFavorite);
        }

    }
}
