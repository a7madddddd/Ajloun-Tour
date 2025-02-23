using Ajloun_Tour.Models;
using Ajloun_Tour.DTOs2.BookingOptionsDTOs;
namespace Ajloun_Tour.Reposetories
{
    public interface IBookingOptionRepository
    {
        Task<IEnumerable<BookingOptionsDTO>> GetAllAsync();
        Task<BookingOptionsDTO> GetByIdAsync(int id);
        Task<BookingOptionsDTO> CreateAsync(CreateBookingOption createBookingOption);
        Task<BookingOptionsDTO> UpdateAsync(int id, CreateBookingOption createBookingOption);
        Task DeleteAsync(int id);
    }
}
