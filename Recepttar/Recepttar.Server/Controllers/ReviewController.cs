using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Constants;
using Recepttar.Server.HelperMethods;
using Recepttar.Server.Models;

namespace Recepttar.Server.Controllers
{
    [ApiController()]
    [Route("reviews/")]
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;
        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPatch("{reviewId}")]
        public IActionResult UpdateReview([FromForm] DTO.ReviewsDTO.PatchReview updates, int reviewId)
        {
            // Unauthorized access (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var review = _context.Review.FirstOrDefault(d => d.Id == reviewId);

            // If review not found (Status code 404)
            if (review == null)
            {
                return NotFound(new { error = "Review not found" });
            }

            // User is authenticated but not the owner of the review (Status code 403)
            if (review.UserId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    error = "You are not allowed to edit this review"
                });
            }

            // Track if anything was updated
            bool wasUpdated = false;

            if (updates.Stars.HasValue)
            {

                if (updates.Stars.Value < 1 || updates.Stars.Value > 5)
                {
                    return BadRequest(new { error = "Stars must be between 1 and 5" });
                }

                review.Stars = updates.Stars.Value;
                wasUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(updates.Comment))
            {
                review.Comment = updates.Comment;
                wasUpdated = true;
            }

            // Only save if something was actually updated
            if (wasUpdated)
            {
                review.UpdatedAt = DateTime.Now;

                _context.SaveChanges();

                return Ok(new { message = "Review updated" });
            }
            else
            {
                // No changes were made
                return Ok(new { message = "No changes were made to the review" });
            }
        }

        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId) 
        {
            // Unauthorized access (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? userId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var review = _context.Review.FirstOrDefault(d => d.Id == reviewId);

            // If review not found (Status code 404)
            if (review == null)
            {
                return NotFound(new { error = "Review not found" });
            }

            // User is authenticated but not the owner of the review (Status code 403)
            if (review.UserId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    error = "You are not allowed to delete this review"
                });
            }

            _context.Review.Remove(review);

            _context.SaveChanges();

            // If comment was successfully deleted (Status code 204)
            return NoContent();
        }
    }
}