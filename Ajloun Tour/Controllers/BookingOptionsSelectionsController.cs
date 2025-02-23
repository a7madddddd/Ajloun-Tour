using Ajloun_Tour.DTOs2.BookingOptionSelectionDTOs;
using Ajloun_Tour.Implementations;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingOptionsSelectionsController : ControllerBase
    {

        private readonly IBookingOptionsSelectionRepository _bookingOptionsSelectionRepository;

        public BookingOptionsSelectionsController(IBookingOptionsSelectionRepository bookingOptionsSelectionRepository)
        {
            _bookingOptionsSelectionRepository = bookingOptionsSelectionRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingOptionSelectionDTO>>> GetAllBookingOptions()
        {
            var bookingOptions = await _bookingOptionsSelectionRepository.GetAllAsync();
            return Ok(bookingOptions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingOptionSelectionDTO>> GetBookingOptionSelection(int id)
        {
            var bookingOption = await _bookingOptionsSelectionRepository.GetByIdAsync(id);
            if (bookingOption == null)
            {
                return NotFound(new { message = "Booking option Selection not found." });
            }
            return Ok(bookingOption);
        }

        [HttpPost]
        public async Task<ActionResult<BookingOptionSelectionDTO>> AddBookingOptionsSelections([FromBody] CreateBookingOptionsSelection createBookingOptionsSelection)
        {
            if (createBookingOptionsSelection == null)
            {
                return BadRequest(new { message = "Invalid booking option Selection data." });
            }

            var newBookingOptionSelection = await _bookingOptionsSelectionRepository.CreateAsync(createBookingOptionsSelection);
            return CreatedAtAction(nameof(GetBookingOptionSelection), new { id = newBookingOptionSelection.OptionId }, newBookingOptionSelection);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookingOptionSelectionDTO>> UpdateBookingOptionsSelection(int id, [FromBody] CreateBookingOptionsSelection createBookingOptionsSelection)
        {
            if (createBookingOptionsSelection == null)
            {
                return BadRequest(new { message = "Invalid booking option Selection data." });
            }

            var existingOptionSelection = await _bookingOptionsSelectionRepository.GetByIdAsync(id);
            if (existingOptionSelection == null)
            {
                return NotFound(new { message = "Booking option Selection not found." });
            }

            var updatedBookingOptionSelection = await _bookingOptionsSelectionRepository.UpdateAsync(id, createBookingOptionsSelection);
            return Ok(updatedBookingOptionSelection);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingOption(int id)
        {
            var optionSelection = await _bookingOptionsSelectionRepository.GetByIdAsync(id);
            if (optionSelection == null)
            {
                return NotFound(new { message = "Booking option Selection not found." });
            }

            await _bookingOptionsSelectionRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
