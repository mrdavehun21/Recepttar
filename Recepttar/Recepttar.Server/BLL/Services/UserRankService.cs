using Recepttar.Server.BLL.Enums;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.DAL.Interfaces;

namespace Recepttar.Server.BLL.Services
{
    public class UserRankService : IUserRankService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRecipeRepository _recipeRepository;

        public UserRankService(IUserRepository userRepository, IRecipeRepository recipeRepository)
        {
            _userRepository = userRepository;
            _recipeRepository = recipeRepository;
        }

        public async Task EvaluateUserRankAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var recipeCount = await _recipeRepository.CountByUserAsync(userId);

            user.Rank = recipeCount switch
            {
                >= 5 => UserRanksEnum.FoodLegend,
                >= 3 => UserRanksEnum.ChefMaster,
                _ => UserRanksEnum.HomeCook
            };

            await _userRepository.UpdateAsync(user);
        }
    }
}
