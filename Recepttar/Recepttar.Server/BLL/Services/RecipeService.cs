using AutoMapper;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public RecipeService(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<List<RecipeCardDto>> GetRecipesAsync()
        {
            var recipes = await _recipeRepository.GetAllAsync();
            return _mapper.Map<List<RecipeCardDto>>(recipes);
        }

        public async Task<List<RecipeCardDto>> GetRecipesByUserIdAsync(int userId)
        {
            var recipes = await _recipeRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<RecipeCardDto>>(recipes);
        }

        public async Task<(RecipeDto? dto, string? error)> GetRecipeByIdAsync(int recipeId)
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (recipe == null)
            {
                return (null, Messages.Recipe.NotFound);
            }

            return (_mapper.Map<RecipeDto>(recipe), null);
        }

        public async Task<(byte[]? picture, string? error)> GetRecipeImageAsync(int recipeId)
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
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
            recipe.CreatedAt = DateTime.UtcNow;

            using (var stream = new MemoryStream())
            {
                await createDto.DishPicture.CopyToAsync(stream);
                recipe.DishPicture = stream.ToArray();
            }

            recipe.Ingredients = _mapper.Map<List<RecipeIngredient>>(createDto.Ingredients);
            recipe.Steps = _mapper.Map<List<RecipeStep>>(createDto.Steps);

            var created = await _recipeRepository.AddAsync(recipe);
            return (_mapper.Map<RecipeDto>(created), null);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdateRecipeAsync(int recipeId, int userId, UpdateRecipeDto updateDto)
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if(recipe == null)
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
                var ingredients = updateDto.Ingredients.Select(i => _mapper.Map<RecipeIngredient>(i)).ToList();
                await _recipeRepository.ReplaceIngredientsAsync(recipeId, ingredients);
                wasUpdated = true;
            }

            if (updateDto.Steps != null)
            {
                var steps = updateDto.Steps.Select(s => _mapper.Map<RecipeStep>(s)).ToList();
                await _recipeRepository.ReplaceStepsAsync(recipeId, steps);
                wasUpdated = true;
            }

            if (wasUpdated)
            {
                await _recipeRepository.UpdateAsync(recipe);
            }

            return (true, wasUpdated, null);
        }

        public async Task<(bool success, string? error)> RemoveRecipeByIdAsync(int userId, int recipeId)
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (recipe == null)
            {
                return (false, Messages.Recipe.NotFound);
            }

            if (recipe.AuthorId != userId)
            {
                return (false, Messages.Recipe.NotOwner);
            }

            await _recipeRepository.DeleteAsync(recipe);
            return (true, null);
        }

        public async Task<List<RecipeCardDto>> SearchRecipesAsync(SearchQueryDto queryDto)
        {
            var recipes = await _recipeRepository.SearchAsync(queryDto);
            return _mapper.Map<List<RecipeCardDto>>(recipes);
        }
    }
}
