using Microsoft.EntityFrameworkCore;
using Recepttar.Server.BLL.Enums;
using Recepttar.Server.DAL.Data;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Repositories
{
    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly AppDbContext _context;

        public LeaderboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetLeaderboardAsync(LeaderboardSortByEnum sortBy)
        {
            var query = _context.Users
                .Include(u => u.Recipes)
                    .ThenInclude(r => r.Reviews)
                .Include(u => u.Recipes)
                    .ThenInclude(r => r.Favorites);

            var sorted = sortBy switch
            {
                LeaderboardSortByEnum.AvgRating => query.OrderByDescending(u => u.Recipes.SelectMany(r => r.Reviews).Average(rv => (double?)rv.Stars) ?? 0),
                LeaderboardSortByEnum.RecipeCount => query.OrderByDescending(u => u.Recipes.Count),
                _ => query.OrderByDescending(u => u.Recipes.SelectMany(r => r.Favorites).Count())
            };

            return await sorted.Take(3).ToListAsync();
        }
    }
}
