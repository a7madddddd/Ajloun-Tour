using Ajloun_Tour.DTOs.BookingsDTOs;
using Ajloun_Tour.DTOs2.BookingOptionsDTOs;
using Ajloun_Tour.DTOs2.BookingOptionSelectionDTOs;
using Ajloun_Tour.DTOs2.CartItemsDTOs;
using Ajloun_Tour.DTOs2.TourCartDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly MyDbContext _context;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ITourCartRepository _tourCartRepository;
        private readonly IBookingOptionRepository _bookingOptionRepository;

        public BookingRepository(MyDbContext context, ICartItemRepository cartItemRepository, IBookingOptionRepository bookingOptionRepository, ITourCartRepository tourCartRepository)

        {
            _context = context;
            _cartItemRepository = cartItemRepository;
            _bookingOptionRepository = bookingOptionRepository;
            _tourCartRepository = tourCartRepository;
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
                return null;

            var options = await _bookingOptionRepository.GetOptionsByBookingIdAsync(id);
            return MapBookingToDTO(booking, options.ToList());
        }


        public async Task<List<BookingDTO>> GetBookingByUserId(int userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();

            var bookingDTOs = new List<BookingDTO>();

            foreach (var booking in bookings)
            {
                var options = await _bookingOptionRepository.GetOptionsByBookingIdAsync(booking.BookingId);
                bookingDTOs.Add(MapBookingToDTO(booking, options.ToList()));
            }

            return bookingDTOs;
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
        public async Task<Booking> DeleteBookingAsync(int id)
        {
            // Find the booking by its ID
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return null; // Return null if booking not found
            }

            // Delete associated booking options (BookingOptionSelections)
            var bookingOptions = await _context.BookingOptionSelections
                .Where(os => os.BookingId == id)
                .ToListAsync();

            if (bookingOptions.Any())
            {
                _context.BookingOptionSelections.RemoveRange(bookingOptions); // Remove associated options
            }

            // Remove booking reference from cart items
            var cartItems = await _context.CartItems
                .Where(ci => ci.BookingId == id)
                .ToListAsync();

            foreach (var cartItem in cartItems)
            {
                cartItem.BookingId = null; // Clear the booking reference
                _context.Entry(cartItem).State = EntityState.Modified;
            }

            // Finally, remove the booking
            _context.Bookings.Remove(booking);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return booking; // Return the deleted booking
        }


        public async Task<BookingOptionsDTO> GetOrCreateBookingOptionAsync(string optionName, decimal optionPrice)
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
            };
        }
        public async Task<Booking> CreateBookingAsync(int userId, Booking newBooking)
        {
            // Check if the user already has bookings
            bool isFirstBooking = !await _context.Bookings.AnyAsync(b => b.UserId == userId);

            // Create the booking
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            // If it's the user's first booking, create a cart for them
            if (isFirstBooking)
            {
                // Automatically create a cart for the user when it's their first booking
                await _tourCartRepository.AddCart(new CreateTourCart { UserID = userId, Status = "Pending" });
            }

            return newBooking;
        }

        public async Task<BookingDTO> GetBookingFromCartAsync(CreateBooking createBooking)
        {
            try
            {
                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(b =>
                        b.TourId == createBooking.TourId &&
                        b.PackageId == createBooking.PackageId &&
                        b.OfferId == createBooking.OfferId &&
                        b.UserId == createBooking.UserId &&
                        b.Status == createBooking.Status);

                if (booking == null)
                {
                    return null; 
                }

                
                IEnumerable<CartItemDTO> cartItems = new List<CartItemDTO>();

                if (createBooking.CartId.HasValue)
                {
                    cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(createBooking.CartId.Value);
                }
                else if (createBooking.CartItemIds?.Count > 0)
                {
                    foreach (var itemId in createBooking.CartItemIds)
                    {
                        var item = await _cartItemRepository.GetCartItemByIdAsync(itemId);
                        if (item != null)
                            ((List<CartItemDTO>)cartItems).Add(item);
                    }
                }
                else
                {
                    throw new ArgumentException("Either CartId or CartItemIds must be provided");
                }

                var existingOptions = await _bookingOptionRepository.GetOptionsByBookingIdAsync(booking.BookingId);

                return MapBookingToDTO(booking, existingOptions.ToList());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving booking", ex);
            }
        }
    }
}



