using Ajloun_Tour.DTOs2.TourCartItemsDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface ITourCartItemsRepository
    {
        Task<IEnumerable<TourCartItemsDTO>> GetCartItems();
        Task<TourCartItemsDTO> GetCartItemsByCartId(int cartItemId);
        Task<TourCartItemsDTO> AddCartItem(CreateCartItemDTO createTourCart);
        Task<TourCartItemsDTO> UpdateCartItem(int cartItemId, CreateCartItemDTO updateCartItemDTO);
        Task DeleteCartItem(int cartItemId);
    }
}
