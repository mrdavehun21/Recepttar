using Recepttar.Server.BLL.Enums;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Interfaces
{
    public interface ILeaderboardRepository
    {
        Task<IEnumerable<User>> GetLeaderboardAsync(LeaderboardSortByEnum sortBy);
    }
}
