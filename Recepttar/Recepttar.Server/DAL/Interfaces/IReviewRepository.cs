using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Interfaces
{
    public interface IReviewRepository
    {
        Task<bool> RecipeExistsAsync(int recipeId);
        Task<List<Review>?> GetRecipeReviewsAsync(int recipeId);
        Task<Review?> GetReviewByIdAsync(int reviewId);
        Task AddReviewAsync(Review review);
        Task<bool> ReviewExistsForUserAsync(int userId, int recipeId);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(Review review);
    }
}
