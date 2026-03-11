using AutoMapper;
using Recepttar.Server.BLL.Common;
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

        public async Task<IEnumerable<ReviewDto>> GetRecipeReviewsAsync(int recipeId)
        {
            if (!await _reviewRepository.RecipeExistsAsync(recipeId))
            {
                return null;
            }

            var reviews = await _reviewRepository.GetRecipeReviewsAsync(recipeId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<Result> AddReviewAsync(int userId, int recipeId, AddReviewDto reviewDto)
        {
            if (!await _reviewRepository.RecipeExistsAsync(recipeId))
            {
                return Result.Failure(Messages.Recipe.NotFound);
            }

            if (await _reviewRepository.ReviewExistsForUserAsync(userId, recipeId))
            {
                return Result.Failure(Messages.Review.AlreadyReviewed);
            }

            if (reviewDto.Stars < MinStars || reviewDto.Stars > MaxStars)
            {
                return Result.Failure(Messages.Review.InvalidStars);
            }

            var review = _mapper.Map<Review>(reviewDto);
            review.RecipeId = recipeId;
            review.UserId = userId;

            await _reviewRepository.AddReviewAsync(review);
            return Result.Success(Messages.Review.Created);
        }
        
        public async Task<ResultT<UpdateResult>> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDto updateDto)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                return ResultT<UpdateResult>.Failure(Messages.Review.NotFound);
            }

            if (review.UserId != userId)
            {
                return ResultT<UpdateResult>.Failure(Messages.Review.NotOwner);
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
                return ResultT<UpdateResult>.Success(new UpdateResult { WasUpdated = true }, Messages.Review.Updated);
            }

            return ResultT<UpdateResult>.Success(new UpdateResult { WasUpdated = false }, Messages.Review.NoChanges);
        }

        public async Task<Result> DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                return Result.Failure(Messages.Review.NotFound);
            }

            if (review.UserId != userId)
            {
                return Result.Failure(Messages.Review.NotOwnerDelete);
            }

            await _reviewRepository.DeleteReviewAsync(review);
            return Result.Success();
        }
    }
}
