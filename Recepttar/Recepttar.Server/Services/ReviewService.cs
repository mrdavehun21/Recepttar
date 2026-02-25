using Recepttar.Server.DTOs.Review;
using Recepttar.Server.Interfaces.Repositories;
using Recepttar.Server.Interfaces.Services;

namespace Recepttar.Server.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<List<ReviewDto>?> GetRecipeReviewsAsync(int recipeId)
        {
            return await _reviewRepository.GetRecipeReviewsAsync(recipeId);
        }

        public async Task<(bool success, string? error)> AddReviewAsync(int userId, int recipeId, AddReviewDto reviewDto)
        {
            return await _reviewRepository.AddReviewAsync(userId, recipeId, reviewDto);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDto updateDto)
        {
            return await _reviewRepository.UpdateReviewAsync(userId, reviewId, updateDto);
        }

        public async Task<(bool success, string? error, bool forbidden)> DeleteReviewAsync(int userId, int reviewId)
        {
            return await _reviewRepository.DeleteReviewAsync(userId, reviewId);
        }
    }
}
