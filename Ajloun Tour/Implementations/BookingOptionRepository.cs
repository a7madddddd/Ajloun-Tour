﻿using Ajloun_Tour.DTOs.BookingsDTOs;
using Ajloun_Tour.DTOs2.BookingOptionsDTOs;
using Ajloun_Tour.DTOs2.BookingOptionSelectionDTOs;
using Ajloun_Tour.DTOs2.CartItemsDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Ajloun_Tour.Repositories.Implementations;
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
                    OptionName = option.OptionName,
                    OptionPrice = option.OptionPrice,
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
                OptionName = option.OptionName,
                OptionPrice = option.OptionPrice,
            };
        }

        public async Task<IEnumerable<BookingOptionSelectionDTO>> GetOptionsByBookingIdAsync(int bookingId)
        {
            return await _context.BookingOptionSelections
                .Where(os => os.BookingId == bookingId)
                .Join(_context.BookingOptions,
                    os => os.OptionId,
                    o => o.OptionId,
                    (os, o) => new BookingOptionSelectionDTO
                    {
                        SelectionId = os.SelectionId,
                        BookingId = os.BookingId,
                        OptionId = os.OptionId,
                        OptionName = o.OptionName,
                        OptionPrice = o.OptionPrice,
                    })
                .ToListAsync();
        }

        public async Task<BookingOptionsDTO> CreateAsync(CreateBookingOption createBookingOption)
        {
            var newOption = new BookingOption
            {
                OptionName = createBookingOption.OptionName,
                OptionPrice = createBookingOption.OptionPrice,
            };

            _context.BookingOptions.Add(newOption);
            await _context.SaveChangesAsync();

            return new BookingOptionsDTO
            {
                OptionID = newOption.OptionId,
                OptionName = newOption.OptionName,
                OptionPrice = newOption.OptionPrice,
            };
        }

        public async Task<BookingOptionsDTO> UpdateAsync(int id, CreateBookingOption createBookingOption)
        {
            var option = await _context.BookingOptions.FindAsync(id);
            if (option == null)
                return null; 

            option.OptionName = createBookingOption.OptionName;
            option.OptionPrice = createBookingOption.OptionPrice;

            await _context.SaveChangesAsync();

            return new BookingOptionsDTO
            {
                OptionID = option.OptionId,
                OptionName = option.OptionName,
                OptionPrice = option.OptionPrice,
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
