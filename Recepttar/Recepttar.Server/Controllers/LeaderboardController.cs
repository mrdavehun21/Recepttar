using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Leaderboard;
using Recepttar.Server.BLL.Enums;
using Recepttar.Server.BLL.Interfaces;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardservice;

        public LeaderboardController(ILeaderboardService leaderboardservice)
        {
            _leaderboardservice = leaderboardservice;
        }

        [HttpGet("sortoptions")]
        public IActionResult GetSortOptions()
        {
            var options = _leaderboardservice.Getsortoptions();

            return Ok(options);
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaderboardEntryDto>>> GetLeaderboard([FromQuery] LeaderboardSortByEnum sortBy = LeaderboardSortByEnum.FavoriteCount)
        {
            if (!Enum.IsDefined(typeof(LeaderboardSortByEnum), sortBy))
            {
                return BadRequest(Messages.Leaderboard.InvalidOption);
            }

            var result = await _leaderboardservice.GetLeaderboardAsync(sortBy);
            return Ok(result);
        }
    }
}
