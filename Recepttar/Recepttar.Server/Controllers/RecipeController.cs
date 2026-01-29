using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Constants;
using Recepttar.Server.DTO.RecipeDTO;
using Recepttar.Server.DTO.ReviewsDTO;
using Recepttar.Server.HelperMethods;
using Recepttar.Server.Models;
using System.Linq;

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
            var recipesFromDb = _context.Recipe.ToList();

            // Return with every recipe in the recipe table (Status code 200)
            var recipeDto = recipesFromDb.Select(r => new RequestFullRecipe
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Difficulty = r.Difficulty,
                TimeMinutes = r.TimeMinutes,
                Servings = r.Servings,
                IsExpensive = r.IsExpensive,
                IsVegan = r.IsVegan,
                Type = r.Type,
                DishPicture = DishPicturePath.GetPath(r.Id)
            }).ToList();

            return Ok(recipeDto);
        }

        [HttpPost("create")]
        public IActionResult CreateRecipe([FromForm] DTO.RecipeDTO.CreateRecipe newRecipe)
        {
            // Unauthorized access (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var user = _context.User.FirstOrDefault(d => d.Id == UserId);

            // Bad request or missing/invalid data (Status code 400)
            if (newRecipe.TimeMinutes <= 1 ||
                newRecipe.Servings < 1)
            {
                return BadRequest(new { error = "Invalid request body" });
            }

            var recipe = new Recipe()
            {
                AuthorId = user.Id,
                Title = newRecipe.Title,
                Description = newRecipe.Description,
                Difficulty = newRecipe.Difficulty,
                TimeMinutes = newRecipe.TimeMinutes,
                Servings = newRecipe.Servings,
                IsExpensive = newRecipe.IsExpensive,
                IsVegan = newRecipe.IsVegan,
                Type = newRecipe.Type
            };

            if (newRecipe.DishPicture != null)
            {
                using (var stream = new MemoryStream())
                {
                    newRecipe.DishPicture.CopyTo(stream);
                    recipe.DishPicture = stream.ToArray();
                }
            }

            _context.Recipe.Add(recipe);

            _context.SaveChanges();

            foreach(var item in newRecipe.Ingredients)
            {
                var Ingredient = new RecipeIngredients()
                {
                    RecipeId = recipe.Id,
                    IngredientId = item.Id,
                    Quantity = item.Quantity,
                    MeasurementUnit = item.MeasurementUnit
                };
                _context.RecipeIngredients.Add(Ingredient);
            }
            _context.SaveChanges();

            // Recipe added successfully (Status code 201)
            return Created(string.Empty, new { message = "Recipe created" });
        }

        [HttpGet("{recipeId}/image")]
        public IActionResult GetRecipeImage(int recipeId)
        {
            var recipe = _context.Recipe.FirstOrDefault(d => d.Id == recipeId);

            if (recipe == null || recipe.DishPicture == null)
            {
                return NotFound(new { error = "Dish picture not found" });
            }

            byte[] Image = recipe.DishPicture;
            return File(Image, "image/jpg");
        }

        [HttpGet("{recipeId}")]
        public IActionResult GetRecipe(int recipeId)
        {
            var recipe = _context.Recipe.FirstOrDefault(d => d.Id == recipeId);

            // Recipe not found (Status code 404)
            if (recipe == null)
            {
                return NotFound(new { error = "Recipe not found" });
            }

            var Ingredients = _context.RecipeIngredients.Join(_context.Ingredients, RecipeIngredients => RecipeIngredients.IngredientId, Ingredients => Ingredients.Id, (RecipeIngredients, Ingredients) => new { RecipeIngredients, Ingredients })
                .Select(d => new DTO.RecipeDTO.IngredientsDTO { Id = d.Ingredients.Id, IngredientName = d.Ingredients.Name, Quantity = d.RecipeIngredients.Quantity, MeasurementUnit = d.RecipeIngredients.MeasurementUnit }).ToList();

            var RecipeSteps = _context.RecipeSteps.Where(d => d.RecipeId == recipeId).OrderBy(d => d.StepNumber)
                .Select(d => new RecipeStepsDTO { RecipeStepNumber = d.StepNumber, RecipeStepDescription = d.StepDescription }).ToList();

            // Recipe found by id
            var Recipe = new DTO.RecipeDTO.RequestFullRecipe
            {
                Title = recipe.Title,
                Description = recipe.Description,
                Difficulty = recipe.Difficulty,
                TimeMinutes = recipe.TimeMinutes,
                Servings = recipe.Servings,
                IsExpensive = recipe.IsExpensive,
                IsVegan = recipe.IsVegan,
                Type = recipe.Type,
                DishPicture = DishPicturePath.GetPath(recipeId),
                AuthorId = recipe.AuthorId,
                Ingredients = Ingredients,
                RecipeSteps = RecipeSteps
            };

            return Ok(Recipe);
        }

        [HttpPatch("{recipeId}")]
        public IActionResult UpdateRecipe(int recipeId, [FromForm] DTO.RecipeDTO.PatchRecipe updates)
        {
            // Unauthorized access (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var recipe = _context.Recipe.FirstOrDefault(d => d.Id == recipeId);

            // Recipe not found (Status code 404)
            if (recipe == null)
            {
                return NotFound(new { error = "Recipe not found" });
            }

            // User is authenticated but not the owner of the recipe (Status code 403)
            if (recipe.AuthorId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    error = "You are not allowed to edit this recipe"
                });
            }

            // Track if anything was updated
            bool wasUpdated = false;

            // Update only the fields that were provided
            if (!string.IsNullOrWhiteSpace(updates.Title))
            {
                recipe.Title = updates.Title;
                wasUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(updates.Description))
            {
                recipe.Description = updates.Description;
                wasUpdated = true;
            }

            if (updates.Difficulty.HasValue)
            {
                recipe.Difficulty = updates.Difficulty.Value;
                wasUpdated = true;
            }

            if (updates.TimeMinutes.HasValue)
            {
                recipe.TimeMinutes = updates.TimeMinutes.Value;
                wasUpdated = true;
            }

            if (updates.Servings.HasValue)
            {
                recipe.Servings = updates.Servings.Value;
                wasUpdated = true;
            }

            if (updates.IsExpensive.HasValue)
            {
                recipe.IsExpensive = updates.IsExpensive.Value;
                wasUpdated = true;
            }

            if (updates.IsVegan.HasValue)
            {
                recipe.IsVegan = updates.IsVegan.Value;
                wasUpdated = true;
            }

            if (updates.Type.HasValue)
            {
                recipe.Type = updates.Type.Value;
                wasUpdated = true;
            }

            if (updates.DishPicture != null)
            {
                using (var stream = new MemoryStream())
                {
                    updates.DishPicture.CopyTo(stream);
                    recipe.DishPicture = stream.ToArray();
                    wasUpdated = true;
                }
            }

            if(updates.Ingredients != null)
            {
                var ingredients = _context.RecipeIngredients.Where(d => d.RecipeId == recipeId).ExecuteDelete();

                foreach(var item in updates.Ingredients)
                {
                    var recipeIngredient = new RecipeIngredients()
                    {
                        IngredientId = item.Id,
                        RecipeId = recipeId,
                        Quantity = item.Quantity,
                        MeasurementUnit = item.MeasurementUnit
                    };
                    _context.RecipeIngredients.Add(recipeIngredient);
                }

                wasUpdated = true;
            }

            // Only save if something was actually updated
            if (wasUpdated)
            {
                _context.SaveChanges();

                return Ok(new { message = "Recipe updated" });
            }
            else
            {
                // No changes were made
                return Ok(new { message = "No changes were made to the recipe" });
            }
        }

        [HttpDelete("{recipeId}")]
        public IActionResult DeleteRecipe(int recipeId)
        {
            // Unauthorized access (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var recipe = _context.Recipe.FirstOrDefault(d => d.Id == recipeId);

            // Recipe not found (Status code 404)
            if (recipe == null)
            {
                return NotFound(new { error = "Recipe not found" });
            }

            // User is authenticated but not the owner of the recipe (403)
            if (recipe.AuthorId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    error = "You are not allowed to delete this recipe"
                });
            }

            _context.Recipe.Remove(recipe);

            _context.SaveChanges();

            // Successful deletion (Status code 204)
            return NoContent();
        }
        #endregion Recipe viewing

        #region Recipe search
        [HttpGet("search")]
        public IActionResult SearchRecipe([FromQuery] DTO.SearchQueries queries)
        {
            // Example request: /recipes/search?type=dessert&difficulty=easy&isVegan=true&isExpensive=true&search=chocolate&ingredients=1,2

            var recipeQuery = _context.Recipe.AsQueryable();

            if (queries.Type.HasValue)
            {
                recipeQuery = recipeQuery.Where(r => r.Type == queries.Type.Value);
            }

            if (queries.Difficulty.HasValue)
            {
                recipeQuery = recipeQuery.Where(r => r.Difficulty == queries.Difficulty.Value);
            }

            if (queries.IsVegan.HasValue)
            {
                recipeQuery = recipeQuery.Where(r => r.IsVegan == queries.IsVegan.Value);
            }

            if (queries.IsExpensive.HasValue)
            {
                recipeQuery = recipeQuery.Where(r => r.IsExpensive == queries.IsExpensive.Value);
            }

            if (!string.IsNullOrWhiteSpace(queries.Search))
            {
                recipeQuery = recipeQuery.Where(r => r.Title.Contains(queries.Search) || r.Description.Contains(queries.Search));
            }

            if (!string.IsNullOrEmpty(queries.Ingredients))
            {
                var ingredientIds = queries.Ingredients
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                recipeQuery = recipeQuery
                    .Join(_context.RecipeIngredients,
                        r => r.Id,
                        ri => ri.RecipeId,
                        (r, ri) => new { r, ri })
                    .Where(x => ingredientIds.Contains(x.ri.IngredientId))
                    .Select(x => x.r)
                    .Distinct();
            }

            var resultsDto = recipeQuery.Select(r => new RequestFullRecipe
            {
                Id = r.Id,
                DishPicture = DishPicturePath.GetPath(r.Id),
                Title = r.Title,
                Description = r.Description,
                Difficulty = r.Difficulty,
                TimeMinutes = r.TimeMinutes,
                Servings = r.Servings,
                IsExpensive = r.IsExpensive,
                IsVegan = r.IsVegan,
                Type = r.Type,
                AuthorId = r.AuthorId
            }).ToList();

            // If found recipe, return (Status code 200)
            return Ok(resultsDto);
        }
        #endregion Recipe search

        #region Reviews
        [HttpGet("{recipeId}/reviews")]
        public IActionResult Getreviews(int recipeId)
        {
            var recipe = _context.Recipe.FirstOrDefault(d => d.Id == recipeId);

            var reviews = _context.Review.Where(r => r.RecipeId == recipeId).OrderByDescending(r => r.CreatedAt).ToList();

            // If recipe not found (Status code 404)
            if (recipe == null)
            {
                return NotFound(new { error = "Recipe not found" });
            }

            // If recipe exists, list all the reviews belonging to it
            var reviewsDto = reviews.Select(r => new GetRecipeReviews
            {
                RecipeId = r.RecipeId,
                UserId = r.UserId,
                Stars = r.Stars,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList();

            return Ok(reviewsDto);
        }

        [HttpPost("{recipeId}/reviews")]
        public IActionResult PostReview([FromForm] DTO.ReviewsDTO.AddReview newReview, int recipeId)
        {
            // Unauthorized access (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            // Check if recipe exists
            var recipe = _context.Recipe.FirstOrDefault(d => d.Id == recipeId);
            if (recipe == null)
            {
                return NotFound(new { error = "Recipe not found" });
            }

            // Bad request or missing/invalid data (Status code 400)
            if (newReview.Stars < 1 || newReview.Stars > 5)
            {
                return BadRequest(new { error = "Invalid request body" });
            }

            var reviewEntity = new Review
            {
                RecipeId = recipeId,
                UserId = userId.Value,
                Stars = newReview.Stars,
                Comment = newReview.Comment,
                CreatedAt = DateTime.Now,
            };

            _context.Review.Add(reviewEntity);

            _context.SaveChanges();

            // Comment added successfully (Status code 201)
            return Created(string.Empty, new { message = "Review added successfully" });
        }
        #endregion Reviews
    }
}
