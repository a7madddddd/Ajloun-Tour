using Ajloun_Tour.DTOs.ReviewsDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IReviewsRepository
    {
        Task<IEnumerable<ReviewsDTO>> GetAllReviews();

        Task<ReviewsDTO> getReviewById(int id);

        Task<List<ReviewsDTO>> getReviewByTourId(int tourId);
        Task<List<ReviewsDTO>> getReviewByPackId(int PackId);
        Task<List<ReviewsDTO>> getReviewByOfferId(int offerId);
        Task<List<ReviewsDTO>> getReviewByProductId(int productId);

        Task<ReviewsDTO> AddReviewAsync(CreateReview createReview);

        Task<ReviewsDTO> UpdateReviews(int id, CreateReview createReview);

        Task DeleteReviewById(int id);
    }
}
