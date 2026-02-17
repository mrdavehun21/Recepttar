using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Constants;
using Recepttar.Server.DTOs.Review;
using Recepttar.Server.Services;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPatch("{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromForm] UpdateReviewDto updateDto)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _reviewService.UpdateReviewAsync(userId.Value, reviewId, updateDto);

            if (!result.success)
            {
                return BadRequest(new { error = result.error });
            }

            if (!result.wasUpdated)
            {
                return Ok(new { message = "No changes were made to the review" });
            }

            return Ok(new { message = "Review updated successfully" });
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            var result = await _reviewService.DeleteReviewAsync(userId.Value, reviewId);

            if (!result.success)
            {
                if (result.forbidden)
                {
                    return StatusCode(403, new { error = result.error });
                }

                return NotFound(new { error = result.error });
            }

            return NoContent();
        }
    }
}