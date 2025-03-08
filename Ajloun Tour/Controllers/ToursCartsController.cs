using Ajloun_Tour.DTOs2.TourCartDTOs;
using Ajloun_Tour.Implementations;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursCartsController : ControllerBase
    {
        private readonly ITourCartRepository _tourCartRepository;

        public ToursCartsController(ITourCartRepository tourCartRepository)
        {
            _tourCartRepository = tourCartRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourCartDTO>>> GetCarts()
        {
            var carts = await _tourCartRepository.GetCarts();
            return Ok(carts);
        }

        [HttpGet("{cartId}")]
        public async Task<ActionResult<TourCartDTO>> GetCartById(int cartId)
        {
            var cart = await _tourCartRepository.GetCartById(cartId);
            if (cart == null)
            {
                return NotFound($"Cart with ID {cartId} not found.");
            }
            return Ok(cart);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<TourCartDTO>> GetCartByUserId(int userId)
        {
            var cart = await _tourCartRepository.GetCartIdByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound($"No cart found for User ID {userId}.");
            }
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<TourCartDTO>> CreateCart(CreateTourCart createTourCart)
        {
            var newCart = await _tourCartRepository.AddCart(createTourCart);
            return CreatedAtAction(nameof(GetCartById), new { cartId = newCart.CartID }, newCart);
        }

        [HttpPut("{cartId}")]
        public async Task<IActionResult> UpdateCart(int cartId, CreateTourCart updateCartDTO)
        {
            var updatedCart = await _tourCartRepository.UpdateCart(cartId, updateCartDTO);
            return Ok(updatedCart);
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            await _tourCartRepository.DeleteCart(cartId);
            return NoContent();
        }
    }
}

