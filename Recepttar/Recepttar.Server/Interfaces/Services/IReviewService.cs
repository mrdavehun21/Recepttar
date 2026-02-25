using Recepttar.Server.DTOs.Review;

namespace Recepttar.Server.Interfaces.Services
{
    public interface IReviewService
    {
        public Task<List<ReviewDto>?> GetRecipeReviewsAsync(int recipeId);

        public Task<(bool success, string? error)> AddReviewAsync(int userId, int recipeId, AddReviewDto reviewDto);

        public Task<(bool success, bool wasUpdated, string? error)> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDto updateDto);

        public Task<(bool success, string? error, bool forbidden)> DeleteReviewAsync(int userId, int reviewId);
    }
}
