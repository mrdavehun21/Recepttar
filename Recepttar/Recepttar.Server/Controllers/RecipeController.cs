using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Constants;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.DTOs.Review;
using Recepttar.Server.Interfaces;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IReviewService _reviewService;

        public RecipeController(IRecipeService recipeService, IReviewService reviewService)
        {
            _recipeService = recipeService;
            _reviewService = reviewService;
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

            var recipes = await _recipeService.GetMyRecipesAsync(userId.Value);

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

            var (dto, error) = await _recipeService.AddRecipeAsync(userId.Value, createDto);

            if (dto == null)
            {
                return BadRequest(error);
            }

            return Created(string.Empty, Messages.Recipe.Created);
        }

        [HttpGet("{recipeId}/image")]
        public async Task<IActionResult> GetRecipeImage(int recipeId)
        {
            var (dto, error) = await _recipeService.GetRecipeByIdAsync(recipeId);

            if (dto == null)
            {
                return NotFound(error);
            }

            var (picture, imgError) = await _recipeService.GetRecipeImageAsync(recipeId);

            if (picture == null)
            {
                return NotFound(imgError);
            }

            return File(picture, "image/jpg");
        }

        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetRecipe(int recipeId)
        {
            var (dto, error) = await _recipeService.GetRecipeByIdAsync(recipeId);

            if (dto == null)
            {
                return NotFound(error);
            }

            return Ok(dto);
        }
        
        [HttpPatch("{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(int recipeId, [FromForm] UpdateRecipeDto updateDto)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (success, wasUpdated, error) = await _recipeService.UpdateRecipeAsync(recipeId, userId.Value, updateDto);

            if(!success)
            {
                return BadRequest(error);
            }

            if(!wasUpdated)
            {
                return Ok(Messages.Recipe.NoChanges);
            }

            return Ok(Messages.Recipe.Updated);
        }

        [HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (success, error) = await _recipeService.RemoveRecipeByIdAsync(userId.Value, recipeId);

            if (!success)
            {
                if (error == Messages.Recipe.NotOwner)
                {
                    return StatusCode(403, error);
                }

                return NotFound(error);
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

            var (success, error) = await _reviewService.AddReviewAsync(userId.Value, recipeId, createDto);

            if (success)
            {
                return Created(string.Empty, Messages.Review.Created);
            }

            switch (error)
            {
                case Messages.Review.AlreadyReviewed:
                    return Conflict(error);

                case Messages.Review.NotFound:
                    return NotFound(error);

                case Messages.Review.InvalidStars:
                    return BadRequest(error);

                default:
                    return StatusCode(500, Messages.Server.Error);
            }
        }

        #endregion Reviews
    }
}
