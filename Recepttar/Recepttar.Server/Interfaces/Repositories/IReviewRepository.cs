using Recepttar.Server.DTOs.Review;

namespace Recepttar.Server.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<List<ReviewDto>?> GetRecipeReviewsAsync(int recipeId);
        Task<(bool success, string? error)> AddReviewAsync(int userId, int recipeId, AddReviewDto reviewDto);
        Task<(bool success, bool wasUpdated, string? error)> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDto updateDto);
        Task<(bool success, string? error, bool forbidden)> DeleteReviewAsync(int userId, int reviewId);
    }
}
