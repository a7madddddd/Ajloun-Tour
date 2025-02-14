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
            };
        }
        public async Task<TestoDTO> AddTestoAsync(CreateTesto createTesto)
        {
            var testo = new Testomonial { 
            
                Message = createTesto.Message,
                UserId = createTesto.UserId,
            };

            _context.Testomonials.Add(testo);
            await _context.SaveChangesAsync();

            return new TestoDTO {
            
                TestomoId = testo.TestomoId,
                Message = testo.Message,
                UserId = testo.UserId,
            };
        }

        public async Task DeleteTestoAsync(int id)
        {
            var test = await _context.Testomonials.FindAsync(id);

            if (test == null)
            {

                throw new Exception("This Testmonial Is Not Defined");
            }

            _context.Testomonials.Remove(test);
            await _context.SaveChangesAsync();
        }


    }
}
