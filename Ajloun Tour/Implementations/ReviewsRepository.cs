using Ajloun_Tour.DTOs.ReviewsDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly MyDbContext _context;

        public ReviewsRepository(MyDbContext context)
        {

            _context = context;
        }
        public async Task<IEnumerable<ReviewsDTO>> GetAllReviews()
        {
            var review = await _context.Reviews.ToListAsync();

            return review.Select(r => new ReviewsDTO
            {

                Id = r.Id,
                TourId = r.TourId,
                UserId = r.UserId,
                Rating = r.Rating,
                Comment = r.Comment,
                Subject = r.Subject,
                CreatedAt = DateTime.UtcNow,
            });
        }
        public async Task<ReviewsDTO> getReviewById(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {

                throw new Exception("This Review Is Not Defined");
            };

            return new ReviewsDTO
            {

                Id = review.Id,
                TourId = review.TourId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                Subject = review.Subject,
                CreatedAt = DateTime.UtcNow,

            };
        }

        public async Task<ReviewsDTO> AddReviewAsync(CreateReview createReview)
        {
            var review = new Review
            {

                TourId = createReview.TourId,
                UserId = createReview.UserId,
                Rating = createReview.Rating,
                Comment = createReview.Comment,
                Subject = createReview.Subject,
                CreatedAt = DateTime.UtcNow,

            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return new ReviewsDTO
            {

                Id = review.Id,
                TourId = review.TourId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                Subject = review.Subject,
                CreatedAt = review.CreatedAt,

            };
        }

        public async Task DeleteReviewById(int id)
        {
            var deletedreview = await _context.Reviews.FindAsync(id);

            if (deletedreview == null) {

                throw new Exception("This Review Is Not Defined");
            };

            _context.Reviews.Remove(deletedreview);
            await _context.SaveChangesAsync();
        }


    }
}
