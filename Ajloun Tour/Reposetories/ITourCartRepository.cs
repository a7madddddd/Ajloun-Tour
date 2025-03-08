using Ajloun_Tour.DTOs2;
using Ajloun_Tour.DTOs2.TourCartDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface ITourCartRepository
    {
        Task<IEnumerable<TourCartDTO>> GetCarts();
        Task<TourCartDTO> GetCartById(int cartId);
        Task<TourCartDTO> AddCart(CreateTourCart createTourCart);
        Task<int?> GetCartIdByUserIdAsync(int userId);

        Task<TourCartDTO> UpdateCart(int cartId, CreateTourCart updateCartDTO);
        Task DeleteCart(int cartId);
    }

}

