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

        [HttpDelete("id")]
        public async void DeleteBookingAsync(int id) {
        
            await _bookingRepository.DeleteBookingAsync(id);
        }
    }
}
