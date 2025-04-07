using Ajloun_Tour.DTOs2.CartItemsDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourCartItemsController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;

        public TourCartItemsController(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetAllCartItems()
        {
            return Ok(await _cartItemRepository.GetAllCartItemsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItemDTO>> GetCartItemById(int id)
        {
            var cartItem = await _cartItemRepository.GetCartItemByIdAsync(id);
            if (cartItem == null)
                return NotFound();

            return Ok(cartItem);
        }

        [HttpGet("cart/{cartId}")]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetCartItemsByCartId(int cartId)
        {
            return Ok(await _cartItemRepository.GetCartItemsByCartIdAsync(cartId));
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDTO>> CreateCartItem(CreateCartItemDTO createCartItemDTO)
        {
            var cartItem = await _cartItemRepository.AddCartItemAsync(createCartItemDTO);
            return CreatedAtAction(nameof(GetCartItemById), new { id = cartItem.CartItemId }, cartItem);
        }

        [HttpPost("AddByProduct")]
        public async Task<ActionResult<CartItemDTO>> AddCartItemByProduct(CreateCartItemWithProductDTO dto)
        {
            var result = await _cartItemRepository.AddCartItemByProductAsync(dto);
            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CartItemDTO>> UpdateCartItem(int id, UpdateCartItemDTO updateCartItemDTO)
        {
            var cartItem = await _cartItemRepository.UpdateCartItemAsync(id, updateCartItemDTO);
            if (cartItem == null)
                return NotFound();

            return Ok(cartItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            await _cartItemRepository.DeleteCartItemAsync(id);
            return NoContent();
        }

        [HttpPut("booking/{bookingId}")]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> UpdateCartItemsBookingId([FromBody] int[] cartItemIds, int bookingId)
        {
            var cartItems = await _cartItemRepository.UpdateCartItemsBookingIdAsync(cartItemIds, bookingId);
            return Ok(cartItems);
        }
    }

}
