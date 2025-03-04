using Ajloun_Tour.DTOs.ProjectsDTOs;
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
                OfferId = r.OfferId,
                PackageId = r.PackageId,
                Rating = r.Rating,
                Comment = r.Comment,
                Subject = r.Subject,
                IsActive = r.IsActive,
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
                PackageId = review.PackageId,
                OfferId = review.OfferId,
                Rating = review.Rating,
                Comment = review.Comment,
                Subject = review.Subject,
                IsActive = review.IsActive,
                CreatedAt = DateTime.UtcNow,

            };
        }
        public async Task<List<ReviewsDTO>> getReviewByTourId(int tourId)
        {
            if (tourId <= 0)
            {
                throw new ArgumentException("Invalid tour ID.");
            }

            var reviews = await _context.Reviews
                .Where(r => r.TourId == tourId)
                .Select(r => new ReviewsDTO
                {
                    Id = r.Id,
                    TourId = r.TourId,
                    UserId = r.UserId,
                    Rating = r.Rating,
                    Subject = r.Subject,
                    Comment = r.Comment,
                    IsActive = r.IsActive,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return reviews;
        }

        public async Task<List<ReviewsDTO>> getReviewByPackId(int PackId)
        {
            if (PackId <= 0)
            {
                throw new ArgumentException("Invalid Pack ID.");
            }

            var reviews = await _context.Reviews
                .Where(r => r.PackageId == PackId)
                .Select(r => new ReviewsDTO
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    PackageId = r.PackageId,
                    Rating = r.Rating,
                    Subject = r.Subject,
                    Comment = r.Comment,
                    IsActive = r.IsActive,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return reviews;
        }

        public async Task<List<ReviewsDTO>> getReviewByOfferId(int offerId)
        {
            if (offerId <= 0)
            {
                throw new ArgumentException("Invalid Offer ID.");
            }

            var reviews = await _context.Reviews
                .Where(r => r.OfferId == offerId)
                .Select(r => new ReviewsDTO
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    OfferId = r.OfferId,
                    Rating = r.Rating,
                    Subject = r.Subject,
                    Comment = r.Comment,
                    IsActive = r.IsActive,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return reviews;
        }


        public async Task<ReviewsDTO> AddReviewAsync(CreateReview createReview)
        {
            var review = new Review
            {
                TourId = createReview.TourId,
                UserId = createReview.UserId,
                PackageId = createReview.PackageId ?? null,
                OfferId = createReview.OfferId ?? null,
                Rating = createReview.Rating,
                Comment = createReview.Comment,
                IsActive = createReview.IsActive ?? false,
                Subject = createReview.Subject,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return new ReviewsDTO
            {
                Id = review.Id,
                TourId = review.TourId,
                UserId = review.UserId,
                OfferId = review.OfferId,
                PackageId = review.PackageId,
                Rating = review.Rating,
                Comment = review.Comment,
                Subject = review.Subject,
                IsActive = review.IsActive,
                CreatedAt = review.CreatedAt,
            };
        }


        public async Task<ReviewsDTO> UpdateReviews(int id, CreateReview createReview)
        {
            var updatedReview = await _context.Reviews.FindAsync(id);

            if (updatedReview == null)
            {

                throw new ArgumentNullException(nameof(updatedReview));
            }

            updatedReview.IsActive = createReview.IsActive ?? updatedReview.IsActive;



            _context.Reviews.Update(updatedReview);
            await _context.SaveChangesAsync();

            return new ReviewsDTO
            {
                IsActive = updatedReview.IsActive,

            };
        }

        public async Task DeleteReviewById(int id)
        {
            var deletedreview = await _context.Reviews.FindAsync(id);

            if (deletedreview == null)
            {

                throw new Exception("This Review Is Not Defined");
            };

            _context.Reviews.Remove(deletedreview);
            await _context.SaveChangesAsync();
        }


    }
}
