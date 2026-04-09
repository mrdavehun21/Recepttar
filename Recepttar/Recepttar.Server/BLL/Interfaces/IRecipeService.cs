using Recepttar.Server.BLL.Common;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.Enums;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeCardDto>> GetRecipesAsync();
        Task<ResultT<RecipeDto>> GetRecipeByIdAsync(int recipeId, LanguagesEnum? language = LanguagesEnum.en);
        Task<IEnumerable<RecipeCardDto>> GetRecipesByUserIdAsync(int userId);
        Task<ResultT<byte[]>> GetRecipeImageAsync(int recipeId);
        Task<ResultT<RecipeDto>> AddRecipeAsync(int userId, CreateRecipeDto createDto);
        Task<ResultT<UpdateResult>> UpdateRecipeAsync(int recipeId, int userId, UpdateRecipeDto updateDto);
        Task<Result> RemoveRecipeByIdAsync(int userId, int recipeId);
        Task<IEnumerable<RecipeCardDto>> SearchRecipesAsync(SearchQueryDto queryDto);
    }
}
