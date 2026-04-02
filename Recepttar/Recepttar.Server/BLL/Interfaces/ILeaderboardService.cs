using Recepttar.Server.BLL.DTOs.Leaderboard;
using Recepttar.Server.BLL.Enums;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface ILeaderboardService
    {
        IEnumerable<string> Getsortoptions();
        Task<IEnumerable<LeaderboardEntryDto>> GetLeaderboardAsync(LeaderboardSortByEnum sortBy);
    }
}
