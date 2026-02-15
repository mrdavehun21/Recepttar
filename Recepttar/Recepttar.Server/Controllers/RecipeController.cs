using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Constants;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.DTOs.Review;
using Recepttar.Server.Services;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;
        private readonly ReviewService _reviewService;

        public RecipeController(RecipeService recipeService, ReviewService reviewService)
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
                return Unauthorized(new { error = "Unauthorized" });
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
                return Unauthorized(new { error = "Unauthorized" });
            }

            var recipeDto = await _recipeService.AddRecipeAsync(userId.Value, createDto);

            if (recipeDto == null)
            {
                return BadRequest(new { error = "Invalid request body" });
            }

            return Created(string.Empty, new { message = "Recipe created" });
        }

        [HttpGet("{recipeId}/image")]
        public async Task<IActionResult> GetRecipeImage(int recipeId)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);

            if (recipe == null)
            {
                return NotFound(new { error = "Recipe not found" });
            }

            var image = await _recipeService.GetRecipeImageAsync(recipeId);

            if (image == null)
            {
                return NotFound(new { error = "Dish picture not found" });
            }

            return File(image, "image/jpg");
        }

        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetRecipe(int recipeId)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);

            if (recipe == null)
            {
                return NotFound(new { error = "Recipe not found" });
            }

            return Ok(recipe);
        }
        
        [HttpPatch("{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(int recipeId, [FromForm] UpdateRecipeDto updateDto)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _recipeService.UpdateRecipeAsync(recipeId, userId.Value, updateDto);

            if(!result.success)
            {
                return BadRequest(new { error = result.error });
            }

            if(!result.wasUpdated)
            {
                return Ok(new { message = "No changes were made to the recipe" });
            }

            return Ok(new { message = "Recipe updated" });
        }

        [HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _recipeService.RemoveRecipeByIdAsync(userId.Value, recipeId);

            if (!result.success)
            {
                if (result.error == "You are not allowed to edit this recipe")
                {
                    return StatusCode(403, new { error = result.error });
                }

                return NotFound(new { error = result.error });
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
                return NotFound(new { error = "Recipe not found" });
            }

            return Ok(reviews);
        }

        [HttpPost("{recipeId}/reviews")]
        public async Task<IActionResult> PostReview([FromForm] AddReviewDto createDto, int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _reviewService.AddReviewAsync(userId.Value, recipeId, createDto);

            if (!result.success)
            {
                return BadRequest(new { error = result.error });
            }

            return Created(string.Empty, new { message = "Review added successfully" });
        }

        #endregion Reviews
    }
}
