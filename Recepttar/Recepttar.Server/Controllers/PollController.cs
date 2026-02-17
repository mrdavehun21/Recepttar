using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Constants;
using Recepttar.Server.DTOs.Poll;
using Recepttar.Server.Services;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
        private readonly PollService _pollService;

        public PollController(PollService pollService)
        {
            _pollService = pollService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePolls()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var polls = await _pollService.GetActivePollsAsync(userId.Value);

            return Ok(polls);
        }

        [HttpPost("{pollId}/vote")]
        public async Task<IActionResult> Vote(int pollId, [FromForm] PollOptionDto voted)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _pollService.AddVoteAsync(userId.Value, pollId, voted.OptionId);

            if (!result.success)
            {
                if (result.error == "User already voted")
                {
                    return Conflict(new { error = result.error });
                }

                if (result.error == "Poll not found")
                {
                    return NotFound(new { error = result.error });
                }

                if (result.error == "Invalid option")
                {
                    return BadRequest(new { error = result.error });
                }
            }

            return Ok(new { message = "Vote recorded" });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePoll([FromForm] PollDto poll)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _pollService.CreatePollAsync(userId.Value, poll);

            if (!result.success)
            {
                return BadRequest(new { error = result.error });
            }

            return Created(String.Empty, new { message = "Poll created" });
        }

        [HttpPatch("{pollId}")]
        public async Task<IActionResult> UpdatePoll(int pollId, [FromForm] PollDto updates)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _pollService.UpdatePollAsync(userId.Value, pollId, updates);

            if (!result.success)
            {
                return BadRequest(new { error = result.error });
            }

            if (!result.wasUpdated)
            {
                return Ok(new { message = "No changes were made to the poll" });
            }

            return Ok(new { message = "Poll updated successfully" });
        }


        [HttpDelete("{pollId}")]
        public async Task<IActionResult> DeletePoll(int pollId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _pollService.DeletePollAsync(userId.Value, pollId);

            if (!result.success)
            {
                if (result.error == "Poll not found")
                {
                    return NotFound(new { error = result.error });
                }

                return StatusCode(403, new { error = result.error });
            }

            return NoContent();
        }
    }
}