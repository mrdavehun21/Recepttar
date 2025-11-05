using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Models;

namespace Recepttar.Server.Controllers
{
    [ApiController()]
    [Route("recipes/")]
    public class RecipeController : Controller
    {
        private readonly AppDbContext _context;
        public RecipeController(AppDbContext context)
        {
            _context = context;
        }

        #region Recipe viewing
        [HttpGet]
        public IActionResult GetAllRecipes()
        {
            // Return with every recipe in the recipe table (Status code 200)
            var recepies = new List<DTO.RecipeDTO.RequestFullRecipe>(); // List all the records in recipes table (TODO)
            return Ok(new DTO.RecipeDTO.RequestFullRecipe());
        }

        [HttpPost("create")]
        public IActionResult CreateRecipe([FromForm] DTO.RecipeDTO.CreateRecipe recipe)
        {
            // Bad request or missing/invalid data (Status code 400)
            return BadRequest(new { error = "Invalid request body" });

            // Unauthorized access (Status code 401)
            return Unauthorized(new { error = "Unauthorized" });

            // Recipe added successfully (Status code 201)
            return Created(string.Empty, new { message = "Recipe created" });
        }

        [HttpGet("{recipeId}")]
        public IActionResult GetRecipe(int recipeId)
        {
            // Recipe not found (Status code 404)
            return NotFound(new { error = "Recipe not found" });

            // Recipe found by id
            var Recipe = new DTO.RecipeDTO.RequestFullRecipe(); // Search for recipe (TODO)
            return Ok(Recipe);
        }

        [HttpPut("{recipeId}")]
        public IActionResult UpdateRecipe(int recipeId)
        {
            // Recipe not found (Status code 404)
            return NotFound(new { error = "Recipe not found" });

            // Unauthorized access (Status code 401)
            return Unauthorized(new { error = "Unauthorized" });

            // Invalid request body (Status code 400)
            return BadRequest(new { error = "Invalid request body" });

            // Successful update (Status code 200)
            return Ok(new { message = "Recipe updated" });
        }

        [HttpDelete("{recipeId}")]
        public IActionResult DeleteRecipe(int recipeId)
        {
            // Recipe not found (Status code 404)
            return NotFound(new { error = "Recipe not found" });

            // Unauthorized access (Status code 401)
            return Unauthorized(new { error = "Unauthorized" });

            // Successful deletion (Status code 200)
            return Ok(new { message = "Recipe deleted successfully" });
        }
        #endregion Recipe viewing

        #region Recipe search
        [HttpGet("search")]
        public IActionResult SearchRecipe([FromQuery] DTO.SearchQueries Queries)
        {
            // Example request: /recipes/search?type=dessert&difficulty=easy&vegan=true&priceCategory=medium&search=chocolate

            // If I make every parameter required and even just one is missing, it'll give a 400 bonding error. I have no control over that error message.

            // Missing or invalid parameters (Status code 400)
            return BadRequest(new { error = "Invalid search parameters" });

            // If found recipe, return (Status code 200)
            var Recipe = new DTO.RecipeDTO.RequestFullRecipe(); // TODO: Try finding it from the database
            return Ok(Recipe);
        }
        #endregion Recipe search

        #region Reviews
        [HttpGet("{recipeId}/reviews")]
        public IActionResult Getreviews(int recipeId)
        {
            // If recipe not found (Status code 404)
            return NotFound(new { error = "Recipe not found" });

            // If recipe exists, list all the reviews belonging to it
            var reviews = new DTO.ReviewsDTO.GetRecipeReviews(); // TODO: Get all the reviews to that recipe
            return Ok(reviews);
        }
        [HttpPost("{recipeId}/reviews")]
        public IActionResult PostReview([FromForm] DTO.ReviewsDTO.AddReview NewReview, int recipeId)
        {
            // If one or more data is missing, it automatically sends a 400 error(?)
            return BadRequest(new { error = "Invalid request body" });

            // In case of unauthorized access (Status code 401)
            return Unauthorized(new { error = "Unauthorized" });

            // Comment added successfully (Status code 201)
            return Created(string.Empty, new { message = "Review added successfully" });
        }
        #endregion Reviews
    }
}
