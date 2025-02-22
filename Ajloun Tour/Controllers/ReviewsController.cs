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
        public async Task <ActionResult<IEnumerable<ReviewsDTO>>> GetAllReviews () {

            var reviews = await _reviewsRepository.GetAllReviews();

            return Ok(reviews);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ReviewsDTO>> GetReviewById(int id) { 
            
            var review = await _reviewsRepository.getReviewById(id);

            return Ok(review);
        
        }

        [HttpPost]
        public async Task<ActionResult> AddReviewAsync(CreateReview createReview) {
        
            var addReview = await _reviewsRepository.AddReviewAsync(createReview);
            return Ok(addReview);
        }

        [HttpDelete("id")]
        public async void DeleteReviewById(int id) { 
        
            await _reviewsRepository.DeleteReviewById(id);
        }
    }
}
