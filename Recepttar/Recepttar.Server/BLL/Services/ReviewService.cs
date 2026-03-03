using AutoMapper;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Review;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public const int MinStars = 1;
        public const int MaxStars = 5;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<List<ReviewDto>?> GetRecipeReviewsAsync(int recipeId)
        {
            if (!await _reviewRepository.RecipeExistsAsync(recipeId))
            {
                return null;
            }

            var reviews = await _reviewRepository.GetRecipeReviewsAsync(recipeId);
            return _mapper.Map<List<ReviewDto>>(reviews);
        }

        public async Task<(bool success, string? error)> AddReviewAsync(int userId, int recipeId, AddReviewDto reviewDto)
        {
            if (!await _reviewRepository.RecipeExistsAsync(recipeId))
            {
                return (false, Messages.Recipe.NotFound);
            }

            if (await _reviewRepository.ReviewExistsForUserAsync(userId, recipeId))
            {
                return (false, Messages.Review.AlreadyReviewed);
            }

            if (reviewDto.Stars < MinStars || reviewDto.Stars > MaxStars)
            {
                return (false, Messages.Review.InvalidStars);
            }

            var review = _mapper.Map<Review>(reviewDto);
            review.RecipeId = recipeId;
            review.UserId = userId;

            await _reviewRepository.AddReviewAsync(review);
            return (true, null);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDto updateDto)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
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
                await _reviewRepository.UpdateReviewAsync(review);
            }

            return (true, wasUpdated, null);
        }

        public async Task<(bool success, string? error, bool forbidden)> DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                return (false, Messages.Review.NotFound, false);
            }

            if (review.UserId != userId)
            {
                return (false, Messages.Review.NotOwnerDelete, true);
            }

            await _reviewRepository.DeleteReviewAsync(review);
            return (true, null, false);
        }
    }
}
