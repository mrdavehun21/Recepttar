using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.DTOs.Review;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.BLL.Constants;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IReviewService _reviewService;
        private readonly IUserRankService _userRankService;

        public RecipeController(IRecipeService recipeService, IReviewService reviewService, IUserRankService userRankService)
        {
            _recipeService = recipeService;
            _reviewService = reviewService;
            _userRankService = userRankService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeService.GetRecipesAsync();

            return Ok(recipes);
        }

        [HttpGet("recipes")]
        public async Task<IActionResult> GetMyRecipes()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var recipes = await _recipeService.GetRecipesByUserIdAsync(userId.Value);

            return Ok(recipes);
        }

        [HttpGet("{userId}/recipes")]
        public async Task<IActionResult> GetRecipesByUserId(int userId)
        {
            var recipes = await _recipeService.GetRecipesByUserIdAsync(userId);

            return Ok(recipes);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRecipe([FromForm] CreateRecipeDto createDto)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var addRecipeResult = await _recipeService.AddRecipeAsync(userId.Value, createDto);

            if (addRecipeResult.Data == null)
            {
                return BadRequest(addRecipeResult.ErrorMessage);
            }

            await _userRankService.EvaluateUserRankAsync(userId.Value);
            return Created(string.Empty, addRecipeResult.SuccessMessage);
        }

        [HttpGet("{recipeId}/image")]
        public async Task<IActionResult> GetRecipeImage(int recipeId)
        {
            var recipeImageResult = await _recipeService.GetRecipeImageAsync(recipeId);

            if (recipeImageResult.Data == null)
            {
                return NotFound(recipeImageResult.ErrorMessage);
            }

            return File(recipeImageResult.Data, "image/jpg");
        }

        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetRecipe(int recipeId)
        {
            var recipeByIdResult = await _recipeService.GetRecipeByIdAsync(recipeId);

            if (recipeByIdResult.Data == null)
            {
                return NotFound(recipeByIdResult.ErrorMessage);
            }

            return Ok(recipeByIdResult.Data);
        }
        
        [HttpPatch("{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(int recipeId, [FromForm] UpdateRecipeDto updateDto)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var updateRecipeResult = await _recipeService.UpdateRecipeAsync(recipeId, userId.Value, updateDto);

            if(!updateRecipeResult.IsSuccess)
            {
                return BadRequest(updateRecipeResult.ErrorMessage);
            }

            return Ok(updateRecipeResult.SuccessMessage);
        }

        [HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var removeRecipeResult = await _recipeService.RemoveRecipeByIdAsync(userId.Value, recipeId);

            if (!removeRecipeResult.IsSuccess)
            {
                if (removeRecipeResult.SuccessMessage == Messages.Recipe.NotOwnerDelete)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = removeRecipeResult.ErrorMessage });
                }

                return NotFound(removeRecipeResult.ErrorMessage);
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRecipe([FromQuery] SearchQueryDto queryDto)
        {
            var result = await _recipeService.SearchRecipesAsync(queryDto);

            return Ok(result);
        }

        #region Reviews

        [HttpGet("{recipeId}/reviews")]
        public async Task<IActionResult> GetReviews(int recipeId)
        {
            var reviews = await _reviewService.GetRecipeReviewsAsync(recipeId);

            if (reviews == null)
            {
                return NotFound(Messages.Recipe.NotFound);
            }

            return Ok(reviews);
        }

        [HttpPost("{recipeId}/reviews")]
        public async Task<IActionResult> PostReview([FromForm] AddReviewDto createDto, int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var addReviewResult = await _reviewService.AddReviewAsync(userId.Value, recipeId, createDto);

            if (addReviewResult.IsSuccess)
            {
                return Created(string.Empty, addReviewResult.SuccessMessage);
            }

            switch (addReviewResult.ErrorMessage)
            {
                case Messages.Review.AlreadyReviewed:
                    return Conflict(addReviewResult.ErrorMessage);

                case Messages.Recipe.NotFound:
                    return NotFound(addReviewResult.ErrorMessage);

                case Messages.Review.InvalidStars:
                    return BadRequest(addReviewResult.ErrorMessage);

                default:
                    return StatusCode(500, Messages.Server.Error);
            }
        }

        #endregion Reviews
    }
}
