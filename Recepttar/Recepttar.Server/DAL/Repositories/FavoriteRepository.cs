using Recepttar.Server.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Recepttar.Server.DAL.Data;
using Recepttar.Server.DAL.Interfaces;

namespace Recepttar.Server.DAL.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Favorite>> GetByUserIdAsync(int userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Recipe)
                    .ThenInclude(r => r.Reviews)
                .ToListAsync();
        }

        public async Task<bool> RecipeExistsAsync(int recipeId)
        {
            return await _context.Recipes.AnyAsync(r => r.Id == recipeId);
        }

        public async Task<Favorite?> GetFavoriteAsync(int userId, int recipeId)
        {
            return await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);
        }

        public async Task AddFavoriteAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoriteAsync(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }

}
