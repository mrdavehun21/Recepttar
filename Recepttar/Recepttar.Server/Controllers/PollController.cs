using Microsoft.AspNetCore.Mvc;

namespace Recepttar.Server.Controllers
{
    [ApiController()]
    [Route("polls/")]
    public class PollController : Controller
    {
        [HttpGet("active")]
        public IActionResult ActivePolls()
        {
            // If no active poll found (Status code 404)
            return NotFound(new { error = "No active poll" });

            // Return found active polls (Status code 200)
            var FoundActivePolls = new List<DTO.PollDTO.ActivePoll>(); // TODO: Find the actual active poll list

            return Ok(FoundActivePolls);
        }

        [HttpPost("{pollId}/vote")]
        public IActionResult AddVote([FromForm] DTO.PollDTO.VotedOption voted, int pollId) // Id is the unique id of the question
        {
            // If poll not found (Status code 404)
            return NotFound(new { error = "Poll not found" });

            // If user chooses an invalid option (Status code 400)
            return BadRequest(new { error = "Invalid option" });

            // After successful vote (Status code 200)
            return Ok(new { message = "Vote recorded" });
        }
    }
}
