using Recepttar.Server.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Recepttar.Server.DAL.Data;
using Recepttar.Server.DAL.Interfaces;

namespace Recepttar.Server.DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> RecipeExistsAsync(int recipeId)
        {
            return await _context.Recipes.AnyAsync(r => r.Id == recipeId);
        }

        public async Task<List<Review>> GetRecipeReviewsAsync(int recipeId)
        {
            return await _context.Reviews
                .Where(r => r.RecipeId == recipeId)
                .OrderByDescending(r => r.CreatedAt)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<bool> ReviewExistsForUserAsync(int userId, int recipeId)
        {
            return await _context.Reviews.AnyAsync(r => r.UserId == userId && r.RecipeId == recipeId);
        }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}
