using Ajloun_Tour.DTOs2.BookingOptionsDTOs;
using Ajloun_Tour.DTOs2.BookingOptionSelectionDTOs;
using Ajloun_Tour.Models;

namespace Ajloun_Tour.Reposetories
{
    public interface IBookingOptionsSelectionRepository
    {
        Task<IEnumerable<BookingOptionSelectionDTO>> GetAllAsync();
        Task<BookingOptionSelectionDTO> GetByIdAsync(int id);
        Task<BookingOptionSelectionDTO> CreateAsync(CreateBookingOptionsSelection createBookingOptionsSelection);
        Task<BookingOptionSelectionDTO> UpdateAsync(int id, CreateBookingOptionsSelection createBookingOptionsSelection);
        Task<List<BookingOptionSelectionDTO>> GetByBookingIdAsync(int bookingId);
        Task DeleteAsync(int id);
    }
}
