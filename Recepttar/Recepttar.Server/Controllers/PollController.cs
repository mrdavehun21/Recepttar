using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.BLL.DTOs.Poll;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.BLL.Constants;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollController(IPollService pollService)
        {
            _pollService = pollService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePolls()
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
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
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (success, error) = await _pollService.AddVoteAsync(userId.Value, pollId, voted.OptionId);

            if (success)
            {
                return Ok(Messages.Poll.Recorded);
            }

            switch (error)
            {
                case Messages.Poll.Voted:
                    return Conflict(error);

                case Messages.Poll.NotFound:
                    return NotFound(error);

                case Messages.Poll.InvalidOption:
                    return BadRequest(error);

                default:
                    return StatusCode(500, Messages.Server.Error);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePoll([FromForm] PollDto poll)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (success, error) = await _pollService.CreatePollAsync(userId.Value, poll);

            if (!success)
            {
                return BadRequest(error);
            }

            return Created(String.Empty, Messages.Poll.Created);
        }

        [HttpPatch("{pollId}")]
        public async Task<IActionResult> UpdatePoll(int pollId, [FromForm] PollDto updates)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (success, wasUpdated, error) = await _pollService.UpdatePollAsync(userId.Value, pollId, updates);

            if (!success)
            {
                return BadRequest(error);
            }

            if (!wasUpdated)
            {
                return Ok(Messages.Poll.NoChanges);
            }

            return Ok(Messages.Poll.Updated);
        }


        [HttpDelete("{pollId}")]
        public async Task<IActionResult> DeletePoll(int pollId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (success, error) = await _pollService.DeletePollAsync(userId.Value, pollId);

            if (success)
            {
                return NoContent();
            }
            
            switch(error)
            {
                case Messages.Poll.NotFound:
                    return NotFound(error);

                case Messages.Poll.NotOwnerDelete:
                    return StatusCode(403, error);

                default:
                    return StatusCode(500, Messages.Server.Error);
            }
        }
    }
}