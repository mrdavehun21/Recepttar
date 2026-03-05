using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.BLL.DTOs.Review;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.BLL.Constants;

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

            var updateReviewResult = await _reviewService.UpdateReviewAsync(userId.Value, reviewId, updateDto);

            if (!updateReviewResult.IsSuccess)
            {
                return BadRequest(updateReviewResult.ErrorMessage);
            }

            return Ok(updateReviewResult.SuccessMessage);
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            if (userId == null)
            {
                return Unauthorized(Messages.Auth.Unauthorized);
            }

            var deleteReviewResult = await _reviewService.DeleteReviewAsync(userId.Value, reviewId);

            if (!deleteReviewResult.IsSuccess)
            {
                if (deleteReviewResult.ErrorMessage == Messages.Review.NotOwnerDelete)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = deleteReviewResult.ErrorMessage });
                }

                return NotFound(deleteReviewResult.ErrorMessage);
            }

            return NoContent();
        }
    }
}