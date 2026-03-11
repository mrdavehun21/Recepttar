using Recepttar.Server.BLL.Common;
using Recepttar.Server.BLL.DTOs.Review;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetRecipeReviewsAsync(int recipeId);

        Task<Result> AddReviewAsync(int userId, int recipeId, AddReviewDto reviewDto);

        Task<ResultT<UpdateResult>> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDto updateDto);

        Task<Result> DeleteReviewAsync(int userId, int reviewId);
    }
}
