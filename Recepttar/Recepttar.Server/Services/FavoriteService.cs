using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Constants;
using Recepttar.Server.Data;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Models;

namespace Recepttar.Server.Services
{
    public class FavoriteService
    {
        private readonly AppDbContext _context;

        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FavoriteRecipeDto>> GetUserFavoritesAsync(int userId)
        {
            return await _context.Favorite
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
            var recipe = await _context.Recipe.FirstOrDefaultAsync(r => r.Id == favoriteRecipeDto.RecipeId);

            if (recipe == null)
            {
                return (false, "Recipe not found");
            }

            var existingFavorite = await _context.Favorite.FirstOrDefaultAsync(f => f.UserId == favoriteRecipeDto.UserId && f.RecipeId == favoriteRecipeDto.RecipeId);

            if (existingFavorite != null)
            {
                return (false, "Recipe already in favorites");
            }

            await _context.Favorite.AddAsync(new Favorite
            {
                UserId = favoriteRecipeDto.UserId,
                RecipeId = favoriteRecipeDto.RecipeId
            });

            await _context.SaveChangesAsync();

            return (true, "Recipe added to favorites");
        }

        public async Task<(bool success, string message)> RemoveFavoriteAsync(int userId, int recipeId)
        {
            var favorite = await _context.Favorite.FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);

            if (favorite == null)
            {
                return (false, "Recipe not in favorites");
            }

            _context.Favorite.Remove(favorite);
            await _context.SaveChangesAsync();

            return (true, "Removed from favorites");
        }

    }
}
