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

        public async Task<IEnumerable<ToursProgramDTO>> GetToursPrograms()
        {
            return await _context.TourPrograms
             .Include(tp => tp.Tour)
             .Select(tp => new ToursProgramDTO
             {
                 ProgramId = tp.ProgramId,
                 TourId = tp.TourId,
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
                DayNumber = tourProgram.DayNumber,
                Title = tourProgram.Title,
                Description = tourProgram.Description,
                ProgramDate = tourProgram.ProgramDate
            };

        }
        public async Task<ToursProgramDTO> GetProgramByTourId(int tourId)
        {
            var program = await _context.TourPrograms
         .Where(p => p.TourId == tourId)
         .Select(p => new ToursProgramDTO
         {
             ProgramId = p.ProgramId,
             TourId = p.TourId,
             TourName = p.Tour.TourName,
             DayNumber = p.DayNumber,
             Description = p.Description,
             Title = p.Title,
             ProgramDate = p.ProgramDate
         })
         .FirstOrDefaultAsync();

            if (program == null)
            {
                throw new Exception("No program found for this tour.");
            }


            return new ToursProgramDTO
            {

                ProgramId = program.ProgramId,
                TourId = program.TourId,
                TourName = program.TourName,
                DayNumber = program.DayNumber,
                Description = program.Description,
                Title = program.Title,
                ProgramDate = program.ProgramDate

            };
        }

        public async Task<PackageWithProgramsDTO?> GetPackageWithProgramsAsync(int packageId)
        {

            var program = await _context.TourPrograms
         .Where(p => p.PackageId == packageId)
         .Select(p => new PackageWithProgramsDTO
         {
             ProgramId = p.ProgramId,

             DayNumber = p.DayNumber,
             Description = p.Description,
             Title = p.Title,
             ProgramDate = p.ProgramDate
         })
         .FirstOrDefaultAsync();

            if (program == null)
            {
                throw new Exception("No program found for this tour.");
            }


            return new PackageWithProgramsDTO
            {

                ProgramId = program.ProgramId,
                PackageId = packageId,
                DayNumber = program.DayNumber,
                Description = program.Description,
                Title = program.Title,
                ProgramDate = program.ProgramDate

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
                ProgramDate = createToursProgram.ProgramDate,

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

