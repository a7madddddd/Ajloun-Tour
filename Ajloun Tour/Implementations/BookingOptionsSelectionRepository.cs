using Ajloun_Tour.DTOs2.BookingOptionsDTOs;
using Ajloun_Tour.DTOs2.BookingOptionSelectionDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class BookingOptionsSelectionRepository : IBookingOptionsSelectionRepository
    {
        private readonly MyDbContext _context;

        public BookingOptionsSelectionRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BookingOptionSelectionDTO>> GetAllAsync()
        {
            return await _context.BookingOptionSelections
                .Select(selection => new BookingOptionSelectionDTO
                {
                    BookingId = selection.BookingId,
                    OptionId = selection.OptionId,
                    SelectionId = selection.SelectionId,
                })
                .ToListAsync();
        }
        public async Task<BookingOptionSelectionDTO> GetByIdAsync(int id)
        {
            var selection = await _context.BookingOptionSelections.FindAsync(id);

            if (selection == null)
                return null; 

            return new BookingOptionSelectionDTO
            {
                BookingId = selection.BookingId,
                OptionId = selection.OptionId,
                SelectionId = selection.SelectionId,
            };
        }

        public async Task<List<BookingOptionSelectionDTO>> GetByBookingIdAsync(int bookingId)
        {
            // Query to fetch all selections for the given BookingId, including related Option entity
            var selections = await _context.BookingOptionSelections
                                            .Where(bos => bos.BookingId == bookingId)
                                            .Include(bos => bos.Option)  // Ensure that related Option entity is loaded
                                            .ToListAsync();

            if (selections == null || selections.Count == 0)
                return null;

            // Map the selections to the BookingOptionSelectionDTO, including Option data
            var selectionDTOs = selections.Select(selection => new BookingOptionSelectionDTO
            {
                BookingId = selection.BookingId,
                OptionId = selection.OptionId,
                SelectionId = selection.SelectionId,
                OptionName = selection.Option?.OptionName,  // Assuming Option entity has Name property
                OptionPrice = selection.Option?.OptionPrice, // Assuming Option entity has Price property
            }).ToList();

            return selectionDTOs;
        }



        public async Task<BookingOptionSelectionDTO> CreateAsync(CreateBookingOptionsSelection createBookingOptionsSelection)
        {
            var bookingExists = await _context.Bookings.AnyAsync(b => b.BookingId == createBookingOptionsSelection.BookingId);
            if (!bookingExists)
            {
                throw new Exception("Booking ID not found.");
            }

            var optionExists = await _context.BookingOptions.AnyAsync(o => o.OptionId == createBookingOptionsSelection.OptionId);
            if (!optionExists)
            {
                throw new Exception("Option ID not found.");
            }

            var newSelection = new BookingOptionSelection
            {
                BookingId = createBookingOptionsSelection.BookingId,
                OptionId= createBookingOptionsSelection.OptionId,
            };

            _context.BookingOptionSelections.Add(newSelection);
            await _context.SaveChangesAsync();

            return new BookingOptionSelectionDTO
            {
                OptionId = newSelection.OptionId,
                BookingId = newSelection.BookingId,
                SelectionId = newSelection.SelectionId,
            };
        }
        public async Task<BookingOptionSelectionDTO> UpdateAsync(int id, CreateBookingOptionsSelection createBookingOptionsSelection)
        {
            var selection = await _context.BookingOptionSelections.FindAsync(id);
            if (selection == null)
                return null; 

            selection.OptionId = createBookingOptionsSelection.OptionId;
            selection.BookingId = createBookingOptionsSelection.BookingId;
            

            await _context.SaveChangesAsync();

            return new BookingOptionSelectionDTO
            {
                OptionId = selection.OptionId,
                BookingId = selection.BookingId,
                SelectionId = selection.SelectionId,
            };
        }

        public async Task DeleteAsync(int id)
        {
            var selection = await _context.BookingOptionSelections.FindAsync(id);
            if (selection == null)
                return;

            _context.BookingOptionSelections.Remove(selection);
            await _context.SaveChangesAsync();
        }

    }
}
