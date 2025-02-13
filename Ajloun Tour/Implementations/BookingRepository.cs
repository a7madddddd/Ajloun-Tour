using Ajloun_Tour.DTOs.BookingsDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly MyDbContext _context;

        public BookingRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BookingDTO>> GetAllBookings()
        {
            var Bookings = await _context.Bookings.ToListAsync();

            return Bookings.Select(b => new BookingDTO
            {

                BookingId = b.BookingId,
                TourId = b.TourId,
                UserId = b.UserId,
                BookingDate = b.BookingDate,
                CreatedAt = b.CreatedAt,
                NumberOfPeople = b.NumberOfPeople,
                TotalPrice = b.TotalPrice,
                Status = b.Status
            });
        }
        public async Task<BookingDTO> GetBookingById(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {

                throw new Exception("This Booking Is Not Defined");
            };

            return new BookingDTO
            {

                BookingId = booking.BookingId,
                TourId = booking.TourId,
                UserId = booking.UserId,
                BookingDate = booking.BookingDate,
                CreatedAt = booking.CreatedAt,
                NumberOfPeople = booking.NumberOfPeople,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status
            };
        }

        public async Task<BookingDTO> AddBookingAsync(CreateBooking createBooking)
        {
            var newBooking = new Booking
            {

                TourId = createBooking.TourId,
                UserId = createBooking.UserId,
                BookingDate = createBooking.BookingDate,
                CreatedAt = createBooking.CreatedAt,
                NumberOfPeople = createBooking.NumberOfPeople,
                TotalPrice = createBooking.TotalPrice,
                Status = createBooking.Status
            };

            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            return new BookingDTO
            {

                BookingId = newBooking.BookingId,
                TourId = newBooking.TourId,
                UserId = newBooking.UserId,
                BookingDate = newBooking.BookingDate,
                CreatedAt = newBooking.CreatedAt,
                NumberOfPeople = newBooking.NumberOfPeople,
                TotalPrice = newBooking.TotalPrice,
                Status = newBooking.Status


            };
        }

        public async Task<BookingDTO> UpdateBookingAsync(int id, CreateBooking createBooking)
        {
            var updeteBooking = await _context.Bookings.FindAsync(id);

            if (updeteBooking == null)
            {

                throw new Exception("This Booking Is Not Defined");
            }


            updeteBooking.TourId = createBooking.TourId ?? updeteBooking.TourId;
            updeteBooking.UserId = createBooking.UserId ?? updeteBooking.UserId;
            updeteBooking.Status = createBooking.Status ?? updeteBooking.Status;
            updeteBooking.NumberOfPeople = ((int?)createBooking.NumberOfPeople) ?? updeteBooking.NumberOfPeople;
            updeteBooking.TotalPrice = ((decimal?)createBooking.TotalPrice) ?? updeteBooking.TotalPrice;
            updeteBooking.BookingDate = ((DateTime?)createBooking.BookingDate) ?? updeteBooking.BookingDate;
            updeteBooking.CreatedAt = createBooking.CreatedAt ?? updeteBooking.CreatedAt;

            _context.Bookings.Update(updeteBooking);
            await _context.SaveChangesAsync();

            return new BookingDTO
            {

                BookingId =updeteBooking.BookingId,
                TourId =updeteBooking.TourId,
                UserId =updeteBooking.UserId,
                BookingDate = updeteBooking.BookingDate,
                CreatedAt = updeteBooking.CreatedAt,
                NumberOfPeople = updeteBooking.NumberOfPeople,
                TotalPrice = updeteBooking.TotalPrice,
                Status = updeteBooking.Status,
                
                
            };


        }
        public async Task DeleteBookingAsync(int id)
        {
            var deletedbooking = await _context.ContactMessages.FindAsync(id);

            if (deletedbooking == null)
            {

                throw new ArgumentNullException(nameof(deletedbooking));

            }

            _context.ContactMessages.Remove(deletedbooking);
            await _context.SaveChangesAsync();

        }

    }
}
