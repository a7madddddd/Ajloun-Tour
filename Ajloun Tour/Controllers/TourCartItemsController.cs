using Ajloun_Tour.DTOs2.TourCartItemsDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourCartItemsController : ControllerBase
    {
        private readonly ITourCartItemsRepository _tourCartItemsRepository;

        public TourCartItemsController(ITourCartItemsRepository tourCartItemsRepository)
        {
            _tourCartItemsRepository = tourCartItemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourCartItemsDTO>>> GetCartItems()
        {
            var cartItems = await _tourCartItemsRepository.GetCartItems();
            return Ok(cartItems);
        }

        [HttpGet("{cartItemId}")]
        public async Task<ActionResult<TourCartItemsDTO>> GetCartItemById(int cartItemId)
        {
            var cartItem = await _tourCartItemsRepository.GetCartItemsByCartId(cartItemId);
            return Ok(cartItem);
        }

        [HttpPost]
        public async Task<ActionResult<TourCartItemsDTO>> AddCartItem(CreateCartItemDTO createCartItemDTO)
        {
            var createdCartItem = await _tourCartItemsRepository.AddCartItem(createCartItemDTO);
            return CreatedAtAction(nameof(GetCartItemById), new { cartItemId = createdCartItem.CartItemId }, createdCartItem);
        }

        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, CreateCartItemDTO updateCartItemDTO)
        {
            var updatedCartItem = await _tourCartItemsRepository.UpdateCartItem(cartItemId, updateCartItemDTO);
            return Ok(updatedCartItem);
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            await _tourCartItemsRepository.DeleteCartItem(cartItemId);
            return NoContent();
        }
    }
}
