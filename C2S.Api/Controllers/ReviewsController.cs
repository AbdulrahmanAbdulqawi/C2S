using C2S.Business.Interfaces;
using C2S.Data.Dtos;
using C2S.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace C2S.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto reviewDto)
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                ServiceId = reviewDto.ServiceId,
                UserId = reviewDto.UserId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            var createdReview = await _reviewService.CreateReviewAsync(review);
            return CreatedAtAction(nameof(GetReviewById), new { id = createdReview.Id }, createdReview);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpGet("service/{serviceId}")]
        public async Task<IActionResult> GetReviewsByServiceId(Guid serviceId)
        {
            var reviews = await _reviewService.GetReviewsByServiceIdAsync(serviceId);
            return Ok(reviews);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReviewsByUserId(Guid userId)
        {
            var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
            return Ok(reviews);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(Guid id, [FromBody] ReviewUpdateDto reviewDto)
        {
            var reviewExists = await GetReviewById(id);
            if (reviewExists != null) return Conflict(reviewExists);

            var review = new Review
            {
                Id = reviewDto.Id,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment
            };

            var updatedReview = await _reviewService.UpdateReviewAsync(review);
            if (updatedReview == null) return NotFound();

            return Ok(updatedReview);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var result = await _reviewService.DeleteReviewAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
