using KosDrive.DTOs;
using KosDrive.Services;
using Microsoft.AspNetCore.Mvc;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RatingsController : ControllerBase
    {
        private readonly RatingService _service;

        public RatingsController(RatingService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> PostRating(CreateRatingDto dto)
        {
            var result = await _service.AddRatingAsync(dto);
            return result ? Ok() : BadRequest();
        }

        [HttpGet("Average/{userId}")]
        public async Task<IActionResult> GetAverage(string userId)
        {
            var avg = await _service.GetAverageRatingAsync(userId);
            return Ok(avg);
        }

        [HttpGet("reviews/{userId}")]
        public async Task<IActionResult> GetUserReviews(string userId)
        {
            var reviews = await _service.GetUserReviewsAsync(userId);
            return Ok(reviews);
        }
    }
}
