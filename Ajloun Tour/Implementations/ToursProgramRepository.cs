using Ajloun_Tour.DTOs2.PackagesProgramDTOs;
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


        public async Task<IEnumerable<TourProgram>> GetAllTourProgramsAsync()
        {
            return await _context.TourPrograms
                .Include(tp => tp.Tour)
                .Include(tp => tp.Program)
                .ToListAsync();
        }

        public async Task<TourProgram?> GetTourProgramByIdAsync(int tourProgramId)
        {
            return await _context.TourPrograms
                .Include(tp => tp.Tour)
                .Include(tp => tp.Program)
                .FirstOrDefaultAsync(tp => tp.TourProgramId == tourProgramId);
        }

        public async Task<IEnumerable<TourProgram>> GetTourProgramsByTourIdAsync(int tourId)
        {
            return await _context.TourPrograms
                .Include(tp => tp.Tour)
                .Include(tp => tp.Program)
                .Where(tp => tp.TourId == tourId)
                .OrderBy(tp => tp.DayNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<TourProgram>> GetTourProgramsByProgramIdAsync(int programId)
        {
            return await _context.TourPrograms
                .Include(tp => tp.Tour)
                .Include(tp => tp.Program)
                .Where(tp => tp.ProgramId == programId)
                .ToListAsync();
        }

        public async Task<TourProgram> CreateTourProgramAsync(TourProgram tourProgram)
        {
            tourProgram.CreatedAt = DateTime.Now;
            tourProgram.UpdatedAt = DateTime.Now;

            _context.TourPrograms.Add(tourProgram);
            await _context.SaveChangesAsync();

            return tourProgram;
        }

        public async Task<TourProgram?> UpdateTourProgramAsync(int tourProgramId, TourProgram tourProgram)
        {
            var existingTourProgram = await _context.TourPrograms.FindAsync(tourProgramId);

            if (existingTourProgram == null)
            {
                return null;
            }

            existingTourProgram.DayNumber = tourProgram.DayNumber;
            existingTourProgram.ProgramDate = tourProgram.ProgramDate;
            existingTourProgram.CustomTitle = tourProgram.CustomTitle;
            existingTourProgram.CustomDescription = tourProgram.CustomDescription;
            existingTourProgram.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return existingTourProgram;
        }

        public async Task<bool> DeleteTourProgramAsync(int tourProgramId)
        {
            var tourProgram = await _context.TourPrograms.FindAsync(tourProgramId);

            if (tourProgram == null)
            {
                return false;
            }

            _context.TourPrograms.Remove(tourProgram);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TourProgramExistsAsync(int tourProgramId)
        {
            return await _context.TourPrograms.AnyAsync(tp => tp.TourProgramId == tourProgramId);
        }
    }
}


