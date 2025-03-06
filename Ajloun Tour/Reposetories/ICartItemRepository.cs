using Ajloun_Tour.DTOs2.CartItemsDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItemDTO>> GetAllCartItemsAsync();
        Task<CartItemDTO> GetCartItemByIdAsync(int id);
        Task<IEnumerable<CartItemDTO>> GetCartItemsByCartIdAsync(int cartId);
        Task<CartItemDTO> AddCartItemAsync(CreateCartItemDTO createCartItemDTO);
        Task<CartItemDTO> UpdateCartItemAsync(int id, UpdateCartItemDTO updateCartItemDTO);
        Task DeleteCartItemAsync(int id);
        Task<IEnumerable<CartItemDTO>> UpdateCartItemsBookingIdAsync(int[] cartItemIds, int bookingId);
    }
}
