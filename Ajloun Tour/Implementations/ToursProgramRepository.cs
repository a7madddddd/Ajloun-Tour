using Ajloun_Tour.DTOs2.ToursProgramDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;
namespace Ajloun_Tour.Implementations
{
    public class ToursProgramRepository : IToursProgramRepository
    {
        private readonly MyDbContext _context;

        public ToursProgramRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToursProgramDTO>> GetToursPrograms()
        {
            return await _context.TourPrograms
             .Include(tp => tp.Tour)
             .Select(tp => new ToursProgramDTO
             {
                 ProgramId = tp.ProgramId,
                 TourId = tp.TourId,
                 TourName = tp.Tour.TourName,
                 DayNumber = tp.DayNumber,
                 Title = tp.Title,
                 Description = tp.Description,
                 ProgramDate = tp.ProgramDate
             })
             .ToListAsync();
        }

        public async Task<ToursProgramDTO> GetTourProgram(int id)
        {
            var tourProgram = await _context.TourPrograms
            .Include(tp => tp.Tour)
            .FirstOrDefaultAsync(tp => tp.ProgramId == id);

            if (tourProgram == null)
                return null!;

            return new ToursProgramDTO
            {
                ProgramId = tourProgram.ProgramId,
                TourId = tourProgram.TourId,
                TourName = tourProgram.Tour.TourName,
                DayNumber = tourProgram.DayNumber,
                Title = tourProgram.Title,
                Description = tourProgram.Description,
                ProgramDate = tourProgram.ProgramDate
            };

        }


        public async Task<ToursProgramDTO> AddToursProgram(CreateToursProgram createToursProgram)
        {
            var tourProgram = new TourProgram
            {
                TourId = createToursProgram.TourId,
                DayNumber = createToursProgram.DayNumber,
                Title = createToursProgram.Title,
                Description = createToursProgram.Description,
                ProgramDate = createToursProgram.ProgramDate
            };

            _context.TourPrograms.Add(tourProgram);
            await _context.SaveChangesAsync();

            return new ToursProgramDTO
            {
                ProgramId = tourProgram.ProgramId,
                TourId = tourProgram.TourId,
                DayNumber = tourProgram.DayNumber,
                Title = tourProgram.Title,
                Description = tourProgram.Description,
                ProgramDate = tourProgram.ProgramDate
            };
        }

        public async Task<ToursProgramDTO> UpdateToursProgram(int id, CreateToursProgram createToursProgram)
        {
            var tourProgram = await _context.TourPrograms.FindAsync(id);
            if (tourProgram == null)
                return null!;

            tourProgram.TourId = createToursProgram.TourId;
            tourProgram.DayNumber = createToursProgram.DayNumber;
            tourProgram.Title = createToursProgram.Title;
            tourProgram.Description = createToursProgram.Description;
            tourProgram.ProgramDate = createToursProgram.ProgramDate;

            await _context.SaveChangesAsync();

            return new ToursProgramDTO
            {
                ProgramId = tourProgram.ProgramId,
                TourId = tourProgram.TourId,
                DayNumber = tourProgram.DayNumber,
                Title = tourProgram.Title,
                Description = tourProgram.Description,
                ProgramDate = tourProgram.ProgramDate
            };
        }
        public async Task DeleteToursProgram(int id)
        {
            var tourProgram = await _context.TourPrograms.FindAsync(id);
            if (tourProgram != null)
            {
                _context.TourPrograms.Remove(tourProgram);
                await _context.SaveChangesAsync();
            }
        }
    }
}
