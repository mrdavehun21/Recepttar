using Recepttar.Server.Constants;
using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Data;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Models;

namespace Recepttar.Server.Services
{
    public class RecipeService
    {
        private readonly AppDbContext _context;

        public RecipeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecipeDto>> GetRecipesAsync()
        {
            return await _context.Recipes
                .Select(recipe => new RecipeDto
                {
                    RecipeId = recipe.Id,
                    Title = recipe.Title,
                    Description = recipe.Description,
                    Difficulty = recipe.Difficulty,
                    TimeMinutes = recipe.TimeMinutes,
                    Servings = recipe.Servings,
                    IsExpensive = recipe.IsExpensive,
                    IsVegan = recipe.IsVegan,
                    Type = recipe.Type,
                    DishPicture = DishPicturePath.GetPath(recipe.Id),
                    AuthorId = recipe.AuthorId,

                    Ingredients = _context.Set<RecipeIngredient>()
                        .Where(ri => ri.RecipeId == recipe.Id)
                        .Select(ri => new IngredientDto
                        {
                            Id = ri.IngredientId,
                            IngredientName = ri.Ingredient.Name,
                            Quantity = ri.Quantity,
                            MeasurementUnit = ri.MeasurementUnit
                        }).ToList(),

                    Steps = _context.Set<RecipeStep>()
                        .Where(rs => rs.RecipeId == recipe.Id)
                        .OrderBy(rs => rs.StepNumber)
                        .Select(rs => new StepDto
                        {
                            RecipeStepNumber = rs.StepNumber,
                            RecipeStepDescription = rs.StepDescription
                        }).ToList()
                }).ToListAsync();
        }

        public async Task<RecipeDto?> AddRecipeAsync(int userId, CreateRecipeDto createDto)
        {
            if (createDto.TimeMinutes <= 0 ||
                createDto.Servings <= 0 ||
                createDto.Ingredients.Count <= 0 ||
                createDto.Steps.Count == 0)
            {
                return null;
            }

            var recipe = new Recipe
            {
                AuthorId = userId,
                Title = createDto.Title,
                Description = createDto.Description,
                Difficulty = createDto.Difficulty,
                TimeMinutes = createDto.TimeMinutes,
                Servings = createDto.Servings,
                IsExpensive = createDto.IsExpensive,
                IsVegan = createDto.IsVegan,
                Type = createDto.Type
            };

            if (createDto.DishPicture != null)
            {
                using var stream = new MemoryStream();
                await createDto.DishPicture.CopyToAsync(stream);
                recipe.DishPicture = stream.ToArray();
            }

            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();

            foreach (var item in createDto.Ingredients)
            {
                await _context.RecipeIngredients.AddAsync(new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = item.Id,
                    Quantity = item.Quantity,
                    MeasurementUnit = item.MeasurementUnit
                });
            }

            foreach (var step in createDto.Steps)
            {
                await _context.RecipeSteps.AddAsync(new RecipeStep
                {
                    RecipeId = recipe.Id,
                    StepNumber = step.RecipeStepNumber,
                    StepDescription = step.RecipeStepDescription
                });
            }

            await _context.SaveChangesAsync();

            return new RecipeDto
            {
                RecipeId = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Difficulty = recipe.Difficulty,
                TimeMinutes = recipe.TimeMinutes,
                Servings = recipe.Servings,
                IsExpensive = recipe.IsExpensive,
                IsVegan = recipe.IsVegan,
                Type = recipe.Type,
                DishPicture = DishPicturePath.GetPath(recipe.Id),
                AuthorId = recipe.AuthorId,
                Ingredients = createDto.Ingredients,
                Steps = createDto.Steps
            };
        }

        public async Task<List<RecipeDto>> GetMyRecipesAsync(int userId)
        {
            return await _context.Recipes
                .Where(recipe => recipe.AuthorId == userId)
                .Select(recipe => new RecipeDto
                {
                    RecipeId = recipe.Id,
                    Title = recipe.Title,
                    Description = recipe.Description,
                    Difficulty = recipe.Difficulty,
                    TimeMinutes = recipe.TimeMinutes,
                    Servings = recipe.Servings,
                    IsExpensive = recipe.IsExpensive,
                    IsVegan = recipe.IsVegan,
                    Type = recipe.Type,
                    DishPicture = DishPicturePath.GetPath(recipe.Id),
                    AuthorId = recipe.AuthorId,

                    Ingredients = _context.RecipeIngredients
                        .Where(ri => ri.RecipeId == recipe.Id)
                        .Select(ri => new IngredientDto
                        {
                            Id = ri.IngredientId,
                            IngredientName = ri.Ingredient.Name,
                            Quantity = ri.Quantity,
                            MeasurementUnit = ri.MeasurementUnit
                        }).ToList(),

                    Steps = _context.RecipeSteps
                        .Where(rs => rs.RecipeId == recipe.Id)
                        .OrderBy(rs => rs.StepNumber)
                        .Select(rs => new StepDto
                        {
                            RecipeStepNumber = rs.StepNumber,
                            RecipeStepDescription = rs.StepDescription
                        }).ToList()
                }).ToListAsync();
        }

        public async Task<RecipeDto?> GetRecipeByIdAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                return null;
            }

            var recipeSteps = await _context.RecipeSteps
                .Where(rs => rs.RecipeId == recipeId)
                .OrderBy(rs => rs.StepNumber)
                .Select(rs => new StepDto
                {
                    RecipeStepNumber = rs.StepNumber,
                    RecipeStepDescription = rs.StepDescription
                })
                .ToListAsync();

            var ingredients = await _context.RecipeIngredients
                .Where(ri => ri.RecipeId == recipeId)
                .Select(ri => new IngredientDto
                {
                    Id = ri.IngredientId,
                    IngredientName = ri.Ingredient.Name,
                    Quantity = ri.Quantity,
                    MeasurementUnit = ri.MeasurementUnit
                }).ToListAsync();

            return new RecipeDto
            {
                RecipeId = recipe.Id,
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
                Ingredients = ingredients,
                Steps = recipeSteps
            };
        }

        public async Task<byte[]?> GetRecipeImageAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null || recipe.DishPicture.Length == 0)
            {
                return null;
            }

            return recipe.DishPicture;
        }


        public async Task<(bool success, bool wasUpdated, string? error)> UpdateRecipeAsync(int recipeId, int userId, UpdateRecipeDto updateDto)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                return (false, false, "Recipe not found");
            }

            if (recipe.AuthorId != userId)
            {
                return (false, false, "You are not allowed to edit this recipe");
            }

            bool wasUpdated = false;

            // Update fields if provided
            if (!string.IsNullOrWhiteSpace(updateDto.Title))
            {
                recipe.Title = updateDto.Title;
                wasUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Description))
            {
                recipe.Description = updateDto.Description;
                wasUpdated = true;
            }

            if (updateDto.Difficulty.HasValue)
            {
                recipe.Difficulty = updateDto.Difficulty.Value;
                wasUpdated = true;
            }

            if (updateDto.TimeMinutes.HasValue)
            {
                recipe.TimeMinutes = updateDto.TimeMinutes.Value;
                wasUpdated = true;
            }

            if (updateDto.Servings.HasValue)
            {
                recipe.Servings = updateDto.Servings.Value;
                wasUpdated = true;
            }

            if (updateDto.IsExpensive.HasValue)
            {
                recipe.IsExpensive = updateDto.IsExpensive.Value;
                wasUpdated = true;
            }

            if (updateDto.IsVegan.HasValue)
            {
                recipe.IsVegan = updateDto.IsVegan.Value;
                wasUpdated = true;
            }

            if (updateDto.Type.HasValue)
            {
                recipe.Type = updateDto.Type.Value;
                wasUpdated = true;
            }

            if (updateDto.DishPicture != null)
            {
                using var stream = new MemoryStream();
                await updateDto.DishPicture.CopyToAsync(stream);
                recipe.DishPicture = stream.ToArray();
                wasUpdated = true;
            }

            // Update Ingredients if provided
            if (updateDto.Ingredients != null)
            {
                await _context.RecipeIngredients.Where(ri => ri.RecipeId == recipeId).ExecuteDeleteAsync();

                foreach (var item in updateDto.Ingredients)
                {
                    _context.RecipeIngredients.Add(new RecipeIngredient
                    {
                        RecipeId = recipeId,
                        IngredientId = item.Id,
                        Quantity = item.Quantity,
                        MeasurementUnit = item.MeasurementUnit
                    });
                }

                wasUpdated = true;
            }

            // Update Steps if provided
            if (updateDto.RecipeSteps != null)
            {
                await _context.RecipeSteps.Where(rs => rs.RecipeId == recipeId).ExecuteDeleteAsync();

                foreach (var item in updateDto.RecipeSteps)
                {
                    _context.RecipeSteps.Add(new RecipeStep
                    {
                        RecipeId = recipeId,
                        StepNumber = item.RecipeStepNumber,
                        StepDescription = item.RecipeStepDescription
                    });
                }

                wasUpdated = true;
            }

            if (wasUpdated)
            {
                await _context.SaveChangesAsync();
            }

            return (true, wasUpdated, null);
        }

        public async Task<(bool success, string? error)> RemoveRecipeByIdAsync(int userId, int recipeId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                return (false, "Recipe not found");
            }

            if (recipe.AuthorId != userId)
            {
                return (false, "You are not allowed to delete this recipe");
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<List<RecipeDto>> SearchRecipesAsync(SearchQueryDto queryDto)
        {
            IQueryable<Recipe> recipeQuery = _context.Recipes.AsQueryable();

            var results = await recipeQuery.ToListAsync();

            var resultsDto = new List<RecipeDto>();

            foreach (var r in results)
            {
                var ingredients = await _context.RecipeIngredients
                    .Where(ri => ri.RecipeId == r.Id)
                    .Select(ri => new IngredientDto
                    {
                        Id = ri.IngredientId,
                        IngredientName = ri.Ingredient.Name,
                        Quantity = ri.Quantity,
                        MeasurementUnit = ri.MeasurementUnit
                    }).ToListAsync();

                var steps = await _context.RecipeSteps
                    .Where(rs => rs.RecipeId == r.Id)
                    .OrderBy(rs => rs.StepNumber)
                    .Select(rs => new StepDto
                    {
                        RecipeStepNumber = rs.StepNumber,
                        RecipeStepDescription = rs.StepDescription
                    }).ToListAsync();

                resultsDto.Add(new RecipeDto
                {
                    RecipeId = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Difficulty = r.Difficulty,
                    TimeMinutes = r.TimeMinutes,
                    Servings = r.Servings,
                    IsExpensive = r.IsExpensive,
                    IsVegan = r.IsVegan,
                    Type = r.Type,
                    DishPicture = DishPicturePath.GetPath(r.Id),
                    AuthorId = r.AuthorId,
                    Ingredients = ingredients,
                    Steps = steps
                });
            }

            return resultsDto;
        }
    }
}
