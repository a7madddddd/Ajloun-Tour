using Ajloun_Tour.DTOs2.BookingOptionsDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingOptionsController : ControllerBase
    {
        private readonly IBookingOptionRepository _bookingOptionRepository;

        public BookingOptionsController(IBookingOptionRepository bookingOptionRepository)
        {
            _bookingOptionRepository = bookingOptionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingOptionsDTO>>> GetAllBookingOptions()
        {
            var bookingOptions = await _bookingOptionRepository.GetAllAsync();
            return Ok(bookingOptions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingOptionsDTO>> GetBookingOption(int id)
        {
            var bookingOption = await _bookingOptionRepository.GetByIdAsync(id);
            if (bookingOption == null)
            {
                return NotFound(new { message = "Booking option not found." });
            }
            return Ok(bookingOption);
        }

        [HttpPost]
        public async Task<ActionResult<BookingOptionsDTO>> AddBookingOptions([FromBody] CreateBookingOption createBookingOption)
        {
            if (createBookingOption == null)
            {
                return BadRequest(new { message = "Invalid booking option data." });
            }

            var newBookingOption = await _bookingOptionRepository.CreateAsync(createBookingOption);
            return CreatedAtAction(nameof(GetBookingOption), new { id = newBookingOption.OptionID }, newBookingOption);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookingOptionsDTO>> UpdateBookingOptions(int id, [FromBody] CreateBookingOption createBookingOption)
        {
            if (createBookingOption == null)
            {
                return BadRequest(new { message = "Invalid booking option data." });
            }

            var existingOption = await _bookingOptionRepository.GetByIdAsync(id);
            if (existingOption == null)
            {
                return NotFound(new { message = "Booking option not found." });
            }

            var updatedBookingOption = await _bookingOptionRepository.UpdateAsync(id, createBookingOption);
            return Ok(updatedBookingOption);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingOption(int id)
        {
            var option = await _bookingOptionRepository.GetByIdAsync(id);
            if (option == null)
            {
                return NotFound(new { message = "Booking option not found." });
            }

            await _bookingOptionRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
