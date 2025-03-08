using Ajloun_Tour.DTOs.BookingsDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingsController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetAllBookingsAsync()
        {

            var Bookings = await _bookingRepository.GetAllBookings();
            return Ok(Bookings);
        }

        [HttpGet("id")]
        public async Task<ActionResult<BookingDTO>> GetBookingById(int id)
        {


            var Booking = await _bookingRepository.GetBookingById(id);

            return Ok(Booking);
        }

        [HttpGet("userId")]
        public async Task<ActionResult<List<BookingDTO>>> GetBookingByUserId([FromQuery] int userId)
        {
            var bookings = await _bookingRepository.GetBookingByUserId(userId);
            return Ok(bookings);
        }


        [HttpGet("From-Cart")]
        public async Task<ActionResult<BookingDTO>> GetBookingsFromCart(
            [FromQuery] int tourId,
            [FromQuery] int userId,
            [FromQuery] int packageId,
            [FromQuery] int offerId,
            [FromQuery] string status,
            [FromQuery] int? cartId = null,
            [FromQuery] List<int>? cartItemIds = null)
        {
            var createBooking = new CreateBooking
            {
                TourId = tourId,
                UserId = userId,
                PackageId = packageId,
                OfferId = offerId,
                Status = status,
                CartId = cartId,
                CartItemIds = cartItemIds ?? new List<int>()
            };

            var booking = await _bookingRepository.GetBookingFromCartAsync(createBooking);

            if (booking == null)
            {
                return NotFound("No booking found for the given criteria.");
            }

            return Ok(booking);
        }




        //[HttpPost("from-cart")]
        //public async Task<ActionResult<BookingDTO>> CreateBookingFromCart(CreateBooking createBooking)
        //{
        //    var booking = await _bookingRepository.CreateBookingFromCartAsync(createBooking);
        //    return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingId }, booking);
        //}

        [HttpPost]
        public async Task<ActionResult<BookingDTO>> AddBookingAsync([FromForm]CreateBooking createBooking)
        {

            var newBooking = await _bookingRepository.AddBookingAsync(createBooking);
            return Ok(newBooking);
        }

        [HttpPost("By PackId")]
        public async Task<ActionResult<BookingDTO>> AddPackBookingAsync([FromForm] CreateBooking createBooking)
        {

            var newBooking = await _bookingRepository.AddPackBookingAsync(createBooking);
            return Ok(newBooking);
        }

        [HttpPost("By Offerid")]
        public async Task<ActionResult<BookingDTO>> AddOfferBookingAsync([FromForm] CreateBooking createBooking)
        {

            var newBooking = await _bookingRepository.AddOfferBookingAsync(createBooking);
            return Ok(newBooking);
        }


        [HttpPut("id")]
        public async Task<ActionResult<BookingDTO>> UpdeteBookingAsync(int id,[FromBody] CreateBooking createBooking)
        {

            var updeteBooking = await _bookingRepository.UpdateBookingAsync(id, createBooking);
            return Ok(updeteBooking);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingAsync(int id)
        {
            var result = await _bookingRepository.DeleteBookingAsync(id);
            if (result == null)
            {
                return NotFound(new { message = "Booking not found" });
            }
            return NoContent(); // Successfully deleted
        }


    }
}
