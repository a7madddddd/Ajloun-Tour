using Ajloun_Tour.DTOs.BookingsDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<BookingDTO>> GetAllBookings();
        Task<BookingDTO> GetBookingById(int id);
        Task<BookingDTO> AddBookingAsync(CreateBooking createBooking);
        Task<BookingDTO> AddPackBookingAsync(CreateBooking createBooking);
        Task<BookingDTO> AddOfferBookingAsync(CreateBooking createBooking);
        Task<BookingDTO> UpdateBookingAsync(int id, CreateBooking createBooking);
        Task DeleteBookingAsync(int id);


    }
}
