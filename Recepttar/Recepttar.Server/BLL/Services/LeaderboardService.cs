using AutoMapper;
using Recepttar.Server.BLL.DTOs.Leaderboard;
using Recepttar.Server.BLL.Enums;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.DAL.Interfaces;

namespace Recepttar.Server.BLL.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly ILeaderboardRepository _repository;
        private readonly IMapper _mapper;

        public LeaderboardService(ILeaderboardRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<string> Getsortoptions()
        {
            return Enum.GetValues<LeaderboardSortByEnum>().Select(u => u.ToString()).ToList();
        }

        public async Task<IEnumerable<LeaderboardEntryDto>> GetLeaderboardAsync(LeaderboardSortByEnum sortBy = LeaderboardSortByEnum.FavoriteCount)
        {
            var users = await _repository.GetLeaderboardAsync(sortBy);
            return _mapper.Map<List<LeaderboardEntryDto>>(users);
        }
    }
}
