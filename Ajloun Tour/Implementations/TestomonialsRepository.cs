using Ajloun_Tour.DTOs.TestoDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class TestomonialsRepository : ITestomonialsRepository
    {
        private readonly MyDbContext _context;

        public TestomonialsRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TestoDTO>> GetAllTestoAsync()
        {
            var testo = await _context.Testomonials.ToListAsync();

            return testo.Select(x => new TestoDTO {
            
                TestomoId = x.TestomoId,
                Message = x.Message,
                UserId = x.UserId,
                Accepted = x.Accepted,
            
            });
        }

        public async Task<TestoDTO> GetTestoById(int id)
        {
            var test = await _context.Testomonials.FindAsync(id);

            if (test == null) {

                throw new Exception("This Testmonial Is Not Defined");
            }

            return new TestoDTO {

                TestomoId = test.TestomoId,
                Message = test.Message,
                UserId = test.UserId,
                Accepted= test.Accepted,
            };
        }
        public async Task<TestoDTO> AddTestoAsync(CreateTesto createTesto)
        {
            var testo = new Testomonial { 
            
                Message = createTesto.Message,
                UserId = createTesto.UserId,
                Accepted = createTesto.Accepted = false,
            };

            _context.Testomonials.Add(testo);
            await _context.SaveChangesAsync();

            return new TestoDTO
            {

                TestomoId = testo.TestomoId,
                Message = testo.Message,
                UserId = testo.UserId,
                Accepted = testo.Accepted 
            };
        }

        public async Task DeleteTestoAsync(int id)
        {
            var testimonial = await _context.Testomonials.FindAsync(id);

            if (testimonial == null)
            {
                throw new KeyNotFoundException($"Testimonial with ID {id} not found.");
            }

            _context.Testomonials.Remove(testimonial);
            await _context.SaveChangesAsync();
        }

        public async Task<UpdateTestoDTO> UpdateTestoById(int id, UpdateTestoDTO updateTestoDto)
        {
            // 1. Retrieve the testimonial by ID
            var testimonial = await _context.Testomonials.FindAsync(id);

            if (testimonial == null)
            {
                // If not found, return null or throw an exception
                throw new KeyNotFoundException($"Testimonial with ID {id} not found.");
            }

            // 2. Update properties
            if (updateTestoDto.Accepted.HasValue)
            {
                testimonial.Accepted = updateTestoDto.Accepted.Value;
            }

            // 3. Save changes back to the database
            await _context.SaveChangesAsync();

            // 4. Return the updated DTO
            return new UpdateTestoDTO
            {
                Accepted = testimonial.Accepted
            };
        }

    }
}
