using Recepttar.Server.Data;
using Recepttar.Server.DTOs.Review;
using Recepttar.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Recepttar.Server.Services
{
    public class ReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewDto>?> GetRecipeReviewsAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                return null;
            }

            var reviews = await _context.Reviews
                .Where(r => r.RecipeId == recipeId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewDto
                {
                    RecipeId = r.RecipeId,
                    UserId = r.UserId,
                    Stars = r.Stars,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                }).ToListAsync();

            return reviews;
        }

        public async Task<(bool success, string? error)> AddReviewAsync(int userId, int recipeId, AddReviewDto reviewDto)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                return (false, "Recipe not found");
            }

            if (reviewDto.Stars < 1 || reviewDto.Stars > 5)
            {
                return (false, "Invalid stars value (1-5)");
            }

            var reviewEntity = new Review
            {
                RecipeId = recipeId,
                UserId = userId,
                Stars = reviewDto.Stars,
                Comment = reviewDto.Comment,
                CreatedAt = DateTime.Now
            };

            await _context.Reviews.AddAsync(reviewEntity);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDto updateDto)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
            {
                return (false, false, "Review not found");
            }

            if (review.UserId != userId)
            {
                return (false, false, "You are not allowed to edit this review");
            }

            bool wasUpdated = false;

            // Update fields if provided
            if (updateDto.Stars.HasValue && updateDto.Stars.Value >= 1 && updateDto.Stars.Value <= 5 && review.Stars != updateDto.Stars.Value)
            {
                review.Stars = updateDto.Stars.Value;
                wasUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Comment) && review.Comment != updateDto.Comment)
            {
                review.Comment = updateDto.Comment;
                wasUpdated = true;
            }

            if (wasUpdated)
            {
                review.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return (true, wasUpdated, null);
        }

        public async Task<(bool success, string? error, bool forbidden)> DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
            {
                return (false, "Review not found", false);
            }

            if (review.UserId != userId)
            {
                return (false, "You are not allowed to delete this review", true);
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return (true, null, false);
        }
    }
}
