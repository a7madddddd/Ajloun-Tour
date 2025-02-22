using Ajloun_Tour.DTOs.ReviewsDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IReviewsRepository
    {
        Task<IEnumerable<ReviewsDTO>> GetAllReviews();

        Task<ReviewsDTO> getReviewById(int id);

        Task <ReviewsDTO> AddReviewAsync(CreateReview createReview);

        Task DeleteReviewById(int id);
    }
}
