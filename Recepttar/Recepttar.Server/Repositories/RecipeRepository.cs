using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Constants;
using Recepttar.Server.Data;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Interfaces.Repositories;
using Recepttar.Server.Models;

namespace Recepttar.Server.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RecipeRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RecipeCardDto>> GetRecipesAsync()
        {
            return await ProjectToCardDto(_context.Recipes).ToListAsync();
        }

        public async Task<List<RecipeCardDto>> GetRecipesByUserIdAsync(int userId)
        {
            return await ProjectToCardDto(_context.Recipes.Where(r => r.AuthorId == userId)).ToListAsync();
        }

        public async Task<(RecipeDto? dto, string? error)> GetRecipeByIdAsync(int recipeId)
        {
            var dto = await ProjectToDto(_context.Recipes.Where(r => r.Id == recipeId)).FirstOrDefaultAsync();
            if (dto == null)
            {
                return (null, Messages.Recipe.NotFound);
            }

            return (dto, null);
        }

        public async Task<(byte[]? picture, string? error)> GetRecipeImageAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                return (null, Messages.Recipe.NotFound);
            }

            return (recipe.DishPicture, null);
        }

        public async Task<(RecipeDto? dto, string? error)> AddRecipeAsync(int userId, CreateRecipeDto createDto)
        {
            if (createDto.TimeMinutes <= 0)
            {
                return (null, Messages.Recipe.InvalidTime);
            }

            if (createDto.Servings <= 0)
            {
                return (null, Messages.Recipe.InvalidServings);
            }

            if (createDto.Ingredients.Count <= 0)
            {
                return (null, Messages.Recipe.NoIngredients);
            }

            if (createDto.Steps.Count == 0)
            {
                return (null, Messages.Recipe.NoSteps);
            }

            if (createDto.DishPicture == null)
            {
                return (null, Messages.Recipe.NoPicture);
            }

            var recipe = _mapper.Map<Recipe>(createDto);
            recipe.AuthorId = userId;

            using (var stream = new MemoryStream())
            {
                await createDto.DishPicture.CopyToAsync(stream);
                recipe.DishPicture = stream.ToArray();
            }

            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();

            foreach (var item in createDto.Ingredients)
            {
                var ingredient = _mapper.Map<RecipeIngredient>(item);
                ingredient.RecipeId = recipe.Id;
                _context.RecipeIngredients.Add(ingredient);
            }

            foreach (var step in createDto.Steps)
            {
                var recipeStep = _mapper.Map<RecipeStep>(step);
                recipeStep.RecipeId = recipe.Id;
                _context.RecipeSteps.Add(recipeStep);
            }

            await _context.SaveChangesAsync();

            return (await ProjectToDto(_context.Recipes.Where(r => r.Id == recipe.Id)).FirstAsync(), null);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdateRecipeAsync(int recipeId, int userId, UpdateRecipeDto updateDto)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                return (false, false, Messages.Recipe.NotFound);
            }

            if (recipe.AuthorId != userId)
            {
                return (false, false, Messages.Recipe.NotOwner);
            }

            bool wasUpdated = false;

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

            if (updateDto.Ingredients != null)
            {
                await _context.RecipeIngredients.Where(ri => ri.RecipeId == recipeId).ExecuteDeleteAsync();
                foreach (var item in updateDto.Ingredients)
                {
                    var ingredient = _mapper.Map<RecipeIngredient>(item);
                    ingredient.RecipeId = recipeId;
                    _context.RecipeIngredients.Add(ingredient);
                }
                wasUpdated = true;
            }

            if (updateDto.Steps != null)
            {
                await _context.RecipeSteps.Where(rs => rs.RecipeId == recipeId).ExecuteDeleteAsync();
                foreach (var item in updateDto.Steps)
                {
                    var step = _mapper.Map<RecipeStep>(item);
                    step.RecipeId = recipeId;
                    _context.RecipeSteps.Add(step);
                }
                wasUpdated = true;
            }

            if (wasUpdated) await _context.SaveChangesAsync();

            return (true, wasUpdated, null);
        }

        public async Task<(bool success, string? error)> RemoveRecipeByIdAsync(int userId, int recipeId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);
            if (recipe == null)
            {
                return (false, Messages.Recipe.NotFound);
            }

            if (recipe.AuthorId != userId)
            {
                return (false, Messages.Recipe.NotOwnerDelete);
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<List<RecipeCardDto>> SearchRecipesAsync(SearchQueryDto queryDto)
        {
            IQueryable<Recipe> recipeQuery = _context.Recipes.AsQueryable();

            if (queryDto.Type.HasValue)
            {
                recipeQuery = recipeQuery.Where(r => r.Type == queryDto.Type.Value);
            }

            if (queryDto.Difficulty.HasValue)
            {
                recipeQuery = recipeQuery.Where(r => r.Difficulty == queryDto.Difficulty.Value);
            }

            if (queryDto.IsVegan.HasValue)
            {
                recipeQuery = recipeQuery.Where(r => r.IsVegan == queryDto.IsVegan.Value);
            }

            if (queryDto.IsExpensive.HasValue)
            {
                recipeQuery = recipeQuery.Where(r => r.IsExpensive == queryDto.IsExpensive.Value);
            }

            if (!string.IsNullOrWhiteSpace(queryDto.Search))
            {
                recipeQuery = recipeQuery.Where(r => r.Title.Contains(queryDto.Search));
            }

            if (!string.IsNullOrWhiteSpace(queryDto.Ingredients))
            {
                var ingredientIds = queryDto.Ingredients
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                recipeQuery = recipeQuery
                    .Join(_context.RecipeIngredients, r => r.Id, ri => ri.RecipeId, (r, ri) => new { r, ri })
                    .Where(x => ingredientIds.Contains(x.ri.IngredientId))
                    .Select(x => x.r)
                    .Distinct();
            }

            return await ProjectToCardDto(recipeQuery).ToListAsync();
        }

        private IQueryable<RecipeCardDto> ProjectToCardDto(IQueryable<Recipe> query)
        {
            return query.Select(r => new RecipeCardDto
            {
                RecipeId = r.Id,
                Title = r.Title,
                Description = r.Description,
                DishPicture = DishPicturePath.GetPath(r.Id),
                AverageRating = _context.Reviews.Where(rv => rv.RecipeId == r.Id).Any()
                    ? (float)Math.Round(_context.Reviews.Where(rv => rv.RecipeId == r.Id).Average(rv => rv.Stars), 1)
                    : 0f,
                ReviewCount = _context.Reviews.Count(rv => rv.RecipeId == r.Id)
            });
        }

        private IQueryable<RecipeDto> ProjectToDto(IQueryable<Recipe> query)
        {
            return query.Select(r => new RecipeDto
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
                CreatedAt = r.CreatedAt,
                DishPicture = DishPicturePath.GetPath(r.Id),
                AuthorId = r.AuthorId,
                Ingredients = r.Ingredients.Select(ri => new IngredientDto
                {
                    Id = ri.IngredientId,
                    IngredientName = ri.Ingredient.Name,
                    Quantity = ri.Quantity,
                    MeasurementUnit = ri.MeasurementUnit
                }).ToList(),
                Steps = r.Steps.OrderBy(rs => rs.StepNumber).Select(rs => new StepDto
                {
                    StepNumber = rs.StepNumber,
                    StepDescription = rs.StepDescription
                }).ToList()
            });
        }
    }
}
