using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Constants;
using Recepttar.Server.DTOs.Review;
using Recepttar.Server.Interfaces;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPatch("{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromForm] UpdateReviewDto updateDto)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (success, wasUpdated, error) = await _reviewService.UpdateReviewAsync(userId.Value, reviewId, updateDto);

            if (!success)
            {
                return BadRequest(error);
            }

            if (!wasUpdated)
            {
                return Ok(Messages.Review.NoChanges);
            }

            return Ok(Messages.Review.Updated);
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var (success, error, forbidden) = await _reviewService.DeleteReviewAsync(userId.Value, reviewId);

            if (!success)
            {
                if (forbidden)
                {
                    return StatusCode(403, error);
                }

                return NotFound(error);
            }

            return NoContent();
        }
    }
}