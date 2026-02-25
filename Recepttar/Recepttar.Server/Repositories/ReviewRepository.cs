using AutoMapper;
using Recepttar.Server.Constants;
using Recepttar.Server.Data;
using Recepttar.Server.Models;
using Recepttar.Server.DTOs.Review;
using Recepttar.Server.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Recepttar.Server.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ReviewDto>?> GetRecipeReviewsAsync(int recipeId)
        {
            var recipeExists = await _context.Recipes.AnyAsync(r => r.Id == recipeId);
            if (!recipeExists)
            {
                return null;
            }

            return await _context.Reviews
                .Where(r => r.RecipeId == recipeId)
                .OrderByDescending(r => r.CreatedAt)
                .Join(_context.Users,
                    r => r.UserId,
                    u => u.Id,
                    (r, u) => new ReviewDto
                    {
                        FullName = u.FullName,
                        ProfilePicture = ProfilePicturePath.GetPath(u.Id),
                        Stars = r.Stars,
                        Comment = r.Comment,
                        CreatedAt = r.CreatedAt,
                        UpdatedAt = r.UpdatedAt
                    })
                .ToListAsync();
        }

        public async Task<(bool success, string? error)> AddReviewAsync(int userId, int recipeId, AddReviewDto reviewDto)
        {
            var recipeExists = await _context.Recipes.AnyAsync(r => r.Id == recipeId);
            if (!recipeExists)
            {
                return (false, Messages.Recipe.NotFound);
            }

            var alreadyReviewed = await _context.Reviews.AnyAsync(r => r.UserId == userId && r.RecipeId == recipeId);
            if (alreadyReviewed)
            {
                return (false, Messages.Review.AlreadyReviewed);
            }

            if (reviewDto.Stars < 1 || reviewDto.Stars > 5)
            {
                return (false, Messages.Review.InvalidStars);
            }

            var review = _mapper.Map<Review>(reviewDto);
            review.RecipeId = recipeId;
            review.UserId = userId;

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDto updateDto)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review == null)
            {
                return (false, false, Messages.Review.NotFound);
            }

            if (review.UserId != userId)
            {
                return (false, false, Messages.Review.NotOwner);
            }

            bool wasUpdated = false;

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
                return (false, Messages.Review.NotFound, false);
            }

            if (review.UserId != userId)
            {
                return (false, Messages.Review.NotOwnerDelete, true);
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return (true, null, false);
        }
    }
}
