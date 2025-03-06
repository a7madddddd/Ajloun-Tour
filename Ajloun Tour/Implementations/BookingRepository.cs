using Ajloun_Tour.DTOs.BookingsDTOs;
using Ajloun_Tour.DTOs2.BookingOptionsDTOs;
using Ajloun_Tour.DTOs2.BookingOptionSelectionDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly MyDbContext _context;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IBookingOptionRepository _bookingOptionRepository;

        public BookingRepository(MyDbContext context, ICartItemRepository cartItemRepository, IBookingOptionRepository bookingOptionRepository)

        {
            _context = context;
            _cartItemRepository = cartItemRepository;
            _bookingOptionRepository = bookingOptionRepository;
        }
        public async Task<IEnumerable<BookingDTO>> GetAllBookings()
        {
            var bookings = await _context.Bookings.ToListAsync();
            var bookingDTOs = new List<BookingDTO>();

            foreach (var booking in bookings)
            {
                var options = await _bookingOptionRepository.GetOptionsByBookingIdAsync(booking.BookingId);
                bookingDTOs.Add(MapBookingToDTO(booking, options.ToList()));
            }

            return bookingDTOs;
        }

        public async Task<BookingDTO> GetBookingByIdAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return null;

            var options = await _bookingOptionRepository.GetOptionsByBookingIdAsync(id);
            return MapBookingToDTO(booking, options.ToList());
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


        public async Task<List<BookingDTO>> GetBookingByUserId(int userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId) // ✅ استبدال BookingId بـ UserId
                .Select(b => new BookingDTO
                {
                    BookingId = b.BookingId,
                    TourId = b.TourId,
                    PackageId = b.PackageId,
                    OfferId = b.OfferId,
                    UserId = b.UserId,
                    BookingDate = b.BookingDate,
                    NumberOfPeople = b.NumberOfPeople,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync(); // ✅ إرجاع قائمة بدلاً من عنصر واحد فقط

            return bookings;
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

        public async Task<BookingDTO> AddPackBookingAsync(CreateBooking createBooking)
        {
            var newBooking = new Booking
            {

                PackageId = createBooking.PackageId,
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
                PackageId = newBooking.PackageId,
                UserId = newBooking.UserId,
                BookingDate = newBooking.BookingDate,
                CreatedAt = newBooking.CreatedAt,
                NumberOfPeople = newBooking.NumberOfPeople,
                TotalPrice = newBooking.TotalPrice,
                Status = newBooking.Status


            };
        }

        public async Task<BookingDTO> AddOfferBookingAsync(CreateBooking createBooking)
        {
            var newBooking = new Booking
            {

                OfferId = createBooking.OfferId,
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
                OfferId = newBooking.OfferId,
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

                BookingId = updeteBooking.BookingId,
                TourId = updeteBooking.TourId,
                UserId = updeteBooking.UserId,
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

        public Task<BookingDTO> CreateBookingFromCartAsync(CreateBooking createBooking)
        {
            throw new NotImplementedException();
        }


        private async Task<BookingOptionsDTO> GetOrCreateBookingOptionAsync(string optionName, decimal optionPrice)
        {
            // Check if option already exists
            var existingOption = await _context.BookingOptions
                .FirstOrDefaultAsync(o => o.OptionName == optionName);

            if (existingOption != null)
            {
                return new BookingOptionsDTO
                {
                    OptionID = existingOption.OptionId,
                    OptionName = existingOption.OptionName,
                    OptionPrice = existingOption.OptionPrice
                };
            }

            // Create new option
            var newOption = new BookingOption
            {
                OptionName = optionName,
                OptionPrice = optionPrice
            };

            _context.BookingOptions.Add(newOption);
            await _context.SaveChangesAsync();

            return new BookingOptionsDTO
            {
                OptionID = newOption.OptionId,
                OptionName = newOption.OptionName,
                OptionPrice = newOption.OptionPrice
            };
        }

        private BookingDTO MapBookingToDTO(Booking booking, List<BookingOptionSelectionDTO> options)
        {
            return new BookingDTO
            {
                BookingId = booking.BookingId,
                TourId = booking.TourId,
                PackageId = booking.PackageId,
                OfferId = booking.OfferId,
                UserId = booking.UserId,
                BookingDate = booking.BookingDate,
                NumberOfPeople = booking.NumberOfPeople,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt,
                BookingOptions = options
            };
        }
    }
}
}
