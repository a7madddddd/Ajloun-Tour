using Ajloun_Tour.DTOs.OffersDTOs;
using Ajloun_Tour.DTOs2.ProgramDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly MyDbContext _context;

        public ProgramRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProgramDTO>> GetAllPrograms()
        {
            var programs = await _context.Programs.ToListAsync();

            return programs.Select(o => new ProgramDTO
            {
                ProgramId = o.ProgramId,
                Title = o.Title,
                DefaultDayNumber = o.DefaultDayNumber,
                Description = o.Description,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt

            });
        }
        public async Task<ProgramDTO> GetProgramById(int id)
        {
            var program = await _context.Programs.FindAsync(id);

            if (program == null)
            {
                throw new Exception("This program Is Not Defined");
            }

            return new ProgramDTO
            {
                ProgramId = program.ProgramId,
                Title = program.Title,
                DefaultDayNumber = program.DefaultDayNumber,
                Description = program.Description,
                CreatedAt = program.CreatedAt,
                UpdatedAt = program.UpdatedAt
            };
        }

        public async Task<ProgramDTO> AddProgramAsync(CreateProgram createProgram)
        {
            if (createProgram == null)
            {
                throw new ArgumentNullException(nameof(createProgram), "The program data cannot be null.");
            }

            // Map CreateProgram to Program entity
            var program = new Models.Program
            {
                Title = createProgram.Title,
                DefaultDayNumber = createProgram.DefaultDayNumber,
                Description = createProgram.Description,
                CreatedAt = createProgram.CreatedAt,
                UpdatedAt = createProgram.UpdatedAt
            };

            // Add the program to the database
            _context.Programs.Add(program);
            await _context.SaveChangesAsync();

            // Map the saved Program entity to ProgramDTO
            return new ProgramDTO
            {
                ProgramId = program.ProgramId,  // Assuming ProgramId is auto-generated
                Title = program.Title,
                DefaultDayNumber = program.DefaultDayNumber,
                Description = program.Description,
                CreatedAt = program.CreatedAt,
                UpdatedAt = program.UpdatedAt
            };
        }

        public async Task<ProgramDTO> UpdateProgramAsync(int id, CreateProgram createProgram)
        {
            var updatedProgram = await _context.Programs.FindAsync(id);

            if (updatedProgram == null)
            {
                throw new KeyNotFoundException("This program is not defined.");
            }

            updatedProgram.Title = createProgram.Title ?? updatedProgram.Title;
            updatedProgram.DefaultDayNumber = createProgram.DefaultDayNumber ?? updatedProgram.DefaultDayNumber;
            updatedProgram.DefaultDayNumber = createProgram.DefaultDayNumber ?? updatedProgram.DefaultDayNumber;
            updatedProgram.CreatedAt = createProgram.CreatedAt ?? updatedProgram.CreatedAt;
            updatedProgram.UpdatedAt = createProgram.UpdatedAt ?? updatedProgram.UpdatedAt;

            _context.Programs.Update(updatedProgram);
            await _context.SaveChangesAsync();

            return new ProgramDTO
            {
                ProgramId = updatedProgram.ProgramId,
                Title = updatedProgram.Title,
                Description = updatedProgram.Description,
                DefaultDayNumber = updatedProgram.DefaultDayNumber,
                CreatedAt = updatedProgram.CreatedAt,
                UpdatedAt = updatedProgram.UpdatedAt,
            };
        }

        public async Task DeleteProgramById(int id)
        {
            var deleteProgram = await _context.Programs.FindAsync(id);

            if (deleteProgram == null)
            {

                throw new Exception("This program Is Not Defined");
            }

            _context.Programs.Remove(deleteProgram);
            await _context.SaveChangesAsync();
        }
    }

}

