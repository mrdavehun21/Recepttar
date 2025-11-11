using Microsoft.AspNetCore.Mvc;

namespace Recepttar.Server.Controllers
{
    [ApiController()]
    [Route("reviews/")]
    public class ReviewController : Controller
    {
        [HttpPut("{reviewId}")]
        public IActionResult UpdateReview([FromForm] DTO.ReviewsDTO.AddReview updatedReview, int reviewId)
        {
            // If invalid request body (Status code 400)
            return BadRequest(new { error = "Invalid request body" });

            // If review not found (Status code 404)
            return NotFound(new { error = "Review not found" });

            // If trying to delete someone elses review, deny (Status code 403)
            return Forbid();

            // If comment was successfully deleted (Status code 204)
            var FreshComment = new DTO.ReviewsDTO.AddReview();
            return Ok(FreshComment);
        }

        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId) 
        {
            // If review not found (Status code 404)
            return NotFound(new { error = "Review not found" });

            // If trying to delete someone elses review, deny (Status code 403)
            return Forbid();

            // If comment was successfully deleted (Status code 204)
            return NoContent();
        }
    }
}