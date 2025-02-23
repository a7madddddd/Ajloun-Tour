using Ajloun_Tour.DTOs2.BookingOptionsDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class BookingOptionRepository : IBookingOptionRepository
    {
        private readonly MyDbContext _context;

        public BookingOptionRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BookingOptionsDTO>> GetAllAsync()
        {
            return await _context.BookingOptions
                .Select(option => new BookingOptionsDTO
                {
                    OptionID = option.OptionId,
                    OptionName = option.OptionName
                })
                .ToListAsync();
        }

        public async Task<BookingOptionsDTO> GetByIdAsync(int id)
        {
            var option = await _context.BookingOptions.FindAsync(id);

            if (option == null)
                return null; // or throw an exception if needed

            return new BookingOptionsDTO
            {
                OptionID = option.OptionId,
                OptionName = option.OptionName
            };
        }


        public async Task<BookingOptionsDTO> CreateAsync(CreateBookingOption createBookingOption)
        {
            var newOption = new BookingOption
            {
                OptionName = createBookingOption.OptionName
            };

            _context.BookingOptions.Add(newOption);
            await _context.SaveChangesAsync();

            return new BookingOptionsDTO
            {
                OptionID = newOption.OptionId,
                OptionName = newOption.OptionName
            };
        }

        public async Task<BookingOptionsDTO> UpdateAsync(int id, CreateBookingOption createBookingOption)
        {
            var option = await _context.BookingOptions.FindAsync(id);
            if (option == null)
                return null; // or throw an exception

            option.OptionName = createBookingOption.OptionName;

            await _context.SaveChangesAsync();

            return new BookingOptionsDTO
            {
                OptionID = option.OptionId,
                OptionName = option.OptionName
            };
        }


        public async Task DeleteAsync(int id)
        {
            var option = await _context.BookingOptions.FindAsync(id);
            if (option == null)
                return;

            _context.BookingOptions.Remove(option);
            await _context.SaveChangesAsync();
        }

    }
}
