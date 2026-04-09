using AutoMapper;
using Recepttar.Server.BLL.Common;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.Enums;
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

        public async Task<IEnumerable<RecipeCardDto>> GetRecipesAsync()
        {
            var recipes = await _recipeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RecipeCardDto>>(recipes);
        }

        public async Task<IEnumerable<RecipeCardDto>> GetRecipesByUserIdAsync(int userId)
        {
            var recipes = await _recipeRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RecipeCardDto>>(recipes);
        }

        public async Task<ResultT<RecipeDto>> GetRecipeByIdAsync(int recipeId, LanguagesEnum? language = LanguagesEnum.en)
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (recipe == null)
            {
                return ResultT<RecipeDto>.Failure(Messages.Recipe.NotFound);
            }

            return ResultT<RecipeDto>.Success(_mapper.Map<RecipeDto>(recipe, opt => opt.Items["lang"] = language), null);
        }

        public async Task<ResultT<byte[]>> GetRecipeImageAsync(int recipeId)
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (recipe == null)
            {
                return ResultT<byte[]>.Failure(Messages.Recipe.NotFound);
            }

            return ResultT<byte[]>.Success(recipe.DishPicture, null);
        }

        public async Task<ResultT<RecipeDto>> AddRecipeAsync(int userId, CreateRecipeDto createDto)
        {
            if (createDto.TimeMinutes <= 0)
            {
                return ResultT<RecipeDto>.Failure(Messages.Recipe.InvalidTime);
            }

            if (createDto.Servings <= 0)
            {
                return ResultT<RecipeDto>.Failure(Messages.Recipe.InvalidServings);
            }

            if (createDto.Ingredients.Count == 0)
            {
                return ResultT<RecipeDto>.Failure(Messages.Recipe.NoIngredients);
            }

            if (createDto.Steps.Count == 0)
            {
                return ResultT<RecipeDto>.Failure(Messages.Recipe.NoSteps);
            }

            if (createDto.DishPicture == null)
            {
                return ResultT<RecipeDto>.Failure(Messages.Recipe.NoPicture);
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
            return ResultT<RecipeDto>.Success(_mapper.Map<RecipeDto>(created), Messages.Recipe.Created);
        }

        public async Task<ResultT<UpdateResult>> UpdateRecipeAsync(int recipeId, int userId, UpdateRecipeDto updateDto)
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if(recipe == null)
            {
                return ResultT<UpdateResult>.Failure(Messages.Recipe.NotFound);
            }

            if (recipe.AuthorId != userId)
            {
                return ResultT<UpdateResult>.Failure(Messages.Recipe.NotOwner);
            }

            if (updateDto.TimeMinutes.HasValue && updateDto.TimeMinutes <= 0)
            {
                return ResultT<UpdateResult>.Failure(Messages.Recipe.InvalidTime);
            }

            if (updateDto.Servings.HasValue && updateDto.Servings <= 0)
            {
                return ResultT<UpdateResult>.Failure(Messages.Recipe.InvalidServings);
            }

            if (updateDto.Ingredients != null && updateDto.Ingredients.Count == 0)
            {
                return ResultT<UpdateResult>.Failure(Messages.Recipe.NoIngredients);
            }

            if (updateDto.Steps != null && updateDto.Steps.Count == 0)
            {
                return ResultT<UpdateResult>.Failure(Messages.Recipe.NoSteps);
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
                return ResultT<UpdateResult>.Success(new UpdateResult { WasUpdated = true }, Messages.Recipe.Updated);
            }

            return ResultT<UpdateResult>.Success(new UpdateResult { WasUpdated = false }, Messages.Recipe.NoChanges);
        }

        public async Task<Result> RemoveRecipeByIdAsync(int userId, int recipeId)
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (recipe == null)
            {
                return Result.Failure(Messages.Recipe.NotFound);
            }

            if (recipe.AuthorId != userId)
            {
                return Result.Failure(Messages.Recipe.NotOwner);
            }

            await _recipeRepository.DeleteAsync(recipe);
            return Result.Success();
        }

        public async Task<IEnumerable<RecipeCardDto>> SearchRecipesAsync(SearchQueryDto queryDto)
        {
            var recipes = await _recipeRepository.SearchAsync(queryDto);
            return _mapper.Map<IEnumerable<RecipeCardDto>>(recipes);
        }
    }
}
