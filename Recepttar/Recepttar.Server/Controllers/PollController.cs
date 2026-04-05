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

        [HttpGet("{userId}/polls")]
        public async Task<IActionResult> GetPollsByUserId(int userId)
        {
            var polls = await _pollService.GetPollsByUserId(userId);

            if(polls.IsSuccess)
            {
                return Ok(polls.Data);
            }

            switch (polls.ErrorMessage)
            {
                case Messages.Auth.UserNotFound:
                    return NotFound(polls.ErrorMessage);

                default:
                    return StatusCode(500, Messages.Server.Error);
            }

        }

        [HttpPost("{pollId}/vote")]
        public async Task<IActionResult> Vote(int pollId, [FromForm] PollOptionDto voted)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var voteResult = await _pollService.AddVoteAsync(userId.Value, pollId, voted.OptionId);

            if (voteResult.IsSuccess)
            {
                return Ok(voteResult.SuccessMessage);
            }

            switch (voteResult.ErrorMessage)
            {
                case Messages.Poll.Voted:
                    return Conflict(voteResult.ErrorMessage);

                case Messages.Poll.NotFound:
                    return NotFound(voteResult.ErrorMessage);

                case Messages.Poll.InvalidOption:
                    return BadRequest(voteResult.ErrorMessage);

                case Messages.Poll.NotActive:
                    return BadRequest(voteResult.ErrorMessage);

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

            var pollResult = await _pollService.CreatePollAsync(userId.Value, poll);

            if (!pollResult.IsSuccess)
            {
                return BadRequest(pollResult.ErrorMessage);
            }

            return Created(string.Empty, pollResult.SuccessMessage);
        }

        [HttpPatch("{pollId}")]
        public async Task<IActionResult> UpdatePoll(int pollId, [FromForm] PollDto updates)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var updateResult = await _pollService.UpdatePollAsync(userId.Value, pollId, updates);

            if (!updateResult.IsSuccess)
            {
                return BadRequest(updateResult.ErrorMessage);
            }

            if (!updateResult.Data.WasUpdated)
            {
                return Ok(updateResult.SuccessMessage);
            }

            return Ok(updateResult.SuccessMessage);
        }


        [HttpDelete("{pollId}")]
        public async Task<IActionResult> DeletePoll(int pollId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var deleteResult = await _pollService.DeletePollAsync(userId.Value, pollId);

            if (deleteResult.IsSuccess)
            {
                return NoContent();
            }
            
            switch(deleteResult.ErrorMessage)
            {
                case Messages.Poll.NotFound:
                    return NotFound(deleteResult.ErrorMessage);

                case Messages.Poll.NotOwnerDelete:
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = deleteResult.ErrorMessage });

                default:
                    return StatusCode(500, Messages.Server.Error);
            }
        }
    }
}