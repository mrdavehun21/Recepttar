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
            var FoundActivePolls = new List<DTO.PollDTO.ActivePoll>()
            {
                new DTO.PollDTO.ActivePoll()
                {
                    Id = 1,
                    Options = new List<DTO.PollDTO.PollOption>()
                    {
                        new DTO.PollDTO.PollOption()
                        {
                            Id = 1,
                            OptionText = "Option1",
                            VoteCount = 1,
                        },
                        new DTO.PollDTO.PollOption()
                        {
                            Id = 2,
                            OptionText = "Option2",
                            VoteCount = 1,
                        },
                        new DTO.PollDTO.PollOption()
                        {
                            Id = 3,
                            OptionText = "Option3",
                            VoteCount = 1,
                        }
                    }
                },
                new DTO.PollDTO.ActivePoll()
                {
                    Id = 2,
                    Options = new List<DTO.PollDTO.PollOption>()
                    {
                        new DTO.PollDTO.PollOption()
                        {
                            Id = 4,
                            OptionText = "Option1",
                            VoteCount = 1,
                        },
                        new DTO.PollDTO.PollOption()
                        {
                            Id = 5,
                            OptionText = "Option2",
                            VoteCount = 1,
                        },
                        new DTO.PollDTO.PollOption()
                        {
                            Id = 6,
                            OptionText = "Option3",
                            VoteCount = 1,
                        }
                    }
                }
            };

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
