using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Constants;
using Recepttar.Server.HelperMethods;
using Recepttar.Server.Models;

namespace Recepttar.Server.Controllers
{
    [ApiController()]
    [Route("polls/")]
    public class PollController : Controller
    {
        private readonly AppDbContext _context;

        public PollController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("active")]
        public IActionResult ActivePolls()
        {
            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if(UserId == null)
            {
                UserId = -1;
            }

            // Return found active polls (Status code 200)
            var FoundActivePolls = _context.Poll.Join(_context.PollOption, pollTable => pollTable.Id, pollOptionTable => pollOptionTable.PollId, (pollTable, pollOptionTable) => new {pollTable, pollOptionTable})
                .GroupBy(d => new { d.pollTable.Id, d.pollTable.Question })
                .Select(g => new
                {
                    id = g.Key.Id,
                    authorId = _context.Poll.First(d => d.Id == g.Key.Id).AuthorId,
                    question = g.Key.Question,
                    options = g.Select(x => new
                    {
                        id = x.pollOptionTable.Id,
                        optionText = x.pollOptionTable.OptionText,
                        voteCount = _context.Vote.Count(d => d.OptionId == x.pollOptionTable.Id && d.PollId == x.pollOptionTable.PollId),
                    }).ToList(),
                    votedOn = _context.Vote.Where(v => v.UserId == UserId && v.PollId == g.Key.Id).Select(v => (int?)v.OptionId).FirstOrDefault()
                });

            if(FoundActivePolls == null)
            {
                // If no active poll found (Status code 404)
                return NotFound(new { error = "No active poll" });
            }

            return Ok(FoundActivePolls);
        }

        [HttpPost("{pollId}/vote")]
        public IActionResult AddVote([FromForm] DTO.PollDTO.VotedOption voted, int pollId) // Id is the unique id of the question
        {
            // Unauthorized access (Status 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            // Get userId
            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            // Don't add if it's already there
            var findVote = _context.Vote.FirstOrDefault(d => d.UserId == UserId.Value && d.PollId == pollId);

            if (findVote != null)
            {
                return Conflict(new { error = "User already voted" });
            }

            var findPoll = _context.Poll.FirstOrDefault(d => d.Id == pollId);

            // If poll not found (Status code 404)
            if(findPoll == null)
            {
                return NotFound(new { error = "Poll not found" });
            }

            var pollOptions = _context.PollOption.Where(d => d.PollId == pollId).Select(d => d.Id).ToList();

            // If user chooses an invalid option (Status code 400)
            if(!pollOptions.Contains(voted.OptionId))
            {
                return BadRequest(new { error = "Invalid option" });
            }

            Vote vote = new Vote()
            {
                UserId = UserId.Value,
                PollId = pollId,
                OptionId = voted.OptionId
            };

            _context.Add(vote);

            _context.SaveChanges();

            // After successful vote (Status code 200)
            return Ok(new { message = "Vote recorded" });
        }

        [HttpPost("create")]
        public IActionResult CreatePoll([FromForm] DTO.PollDTO.ActivePoll poll)
        {
            // Check if user is logged in and has the necessary rank to create a poll
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var UserDetails = _context.User.FirstOrDefault(d => d.Id == UserId);

            if(UserDetails == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            if(UserDetails.Rank == Enums.UserRanksEnum.Hobbi_szakács)
            {
                return BadRequest(new { error = "Rank level too low" });
            }

            if(poll.Options.Count < 2 || poll.Question.Length < 15)
            {
                return BadRequest(new { error = "Missing or incomplete field(s)" });
            }

            // Upload poll to poll table
            Poll newPoll = new Poll()
            {
                AuthorId = UserDetails.Id,
                Question = poll.Question
            };

            _context.Poll.Add(newPoll);

            _context.SaveChanges();

            foreach (var item in poll.Options)
            {
                Models.PollOption pollOption = new PollOption()
                {
                    OptionText = item.OptionText,
                    PollId = newPoll.Id
                };
                _context.PollOption.Add(pollOption);
            }

            _context.SaveChanges();
            return Ok(new { message = "Poll posted successfuly" });
        }

        [HttpDelete("/{pollId}")]
        public IActionResult DeletePoll(int pollId)
        {
            // Check if user is logged in and has the necessary rank to create a poll
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var UserDetails = _context.User.FirstOrDefault(d => d.Id == UserId);

            var PollDetail = _context.Poll.FirstOrDefault(d => d.Id == pollId);

            if (PollDetail == null)
            {
                return BadRequest(new { error = "Poll does not exist" });
            }

            if (UserDetails == null || UserDetails.Id != PollDetail.AuthorId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { error = "You are not allowed to delete this poll" });
            }

            _context.Poll.Remove(PollDetail);

            _context.SaveChanges();
            return StatusCode(StatusCodes.Status200OK, new { message = "Poll removed successfuly" });
        }
    }
}