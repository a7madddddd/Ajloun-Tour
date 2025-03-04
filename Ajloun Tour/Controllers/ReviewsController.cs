using Ajloun_Tour.DTOs.ReviewsDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsRepository _reviewsRepository;

        public ReviewsController(IReviewsRepository reviewsRepository)
        {
            _reviewsRepository = reviewsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewsDTO>>> GetAllReviews()
        {

            var reviews = await _reviewsRepository.GetAllReviews();

            return Ok(reviews);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ReviewsDTO>> GetReviewById(int id)
        {

            var review = await _reviewsRepository.getReviewById(id);

            return Ok(review);
        }

        [HttpGet("getReviewByTourId")]
        public async Task<ActionResult<IEnumerable<ReviewsDTO>>> GetReviewByTourId(int tourId)
        {
            var reviews = await _reviewsRepository.getReviewByTourId(tourId);

            if (reviews == null )
            {
                return NotFound("No reviews found for this tour.");
            }

            return Ok(reviews);
        }

        [HttpGet("getReviewByPackId")]
        public async Task<ActionResult<IEnumerable<ReviewsDTO>>> GetReviewByPackId(int packId)
        {
            var reviews = await _reviewsRepository.getReviewByPackId(packId);

            if (reviews == null)
            {
                return NotFound("No reviews found for this Pack.");
            }

            return Ok(reviews);
        }

        [HttpGet("getReviewByOfferId")]
        public async Task<ActionResult<IEnumerable<ReviewsDTO>>> GetReviewByOfferId(int offerId)
        {
            var reviews = await _reviewsRepository.getReviewByOfferId(offerId);

            if (reviews == null)
            {
                return NotFound("No reviews found for this Offer.");
            }

            return Ok(reviews);
        }

        [HttpPost]
        public async Task<ActionResult> AddReviewAsync([FromForm]CreateReview createReview)
        {

            var addReview = await _reviewsRepository.AddReviewAsync(createReview);
            return Ok(addReview);
        }

        [HttpPut("id")]
        public async Task<ActionResult<ReviewsDTO>> UpdateReviewsById(int id ,[FromBody] CreateReview createReview) {
        
            var updatedRviews = await _reviewsRepository.UpdateReviews(id, createReview);

            return Ok(updatedRviews);
        }

        [HttpDelete("id")]
        public async void DeleteReviewById(int id)
        {

            await _reviewsRepository.DeleteReviewById(id);
        }
    }
}
