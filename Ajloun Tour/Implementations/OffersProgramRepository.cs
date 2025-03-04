using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class OffersProgramRepository : IOffersProgramRepository
    {
        private readonly MyDbContext _context;

        public OffersProgramRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OfferProgram>> GetAllOfferProgramsAsync()
        {
            return await _context.OfferPrograms
               .Include(tp => tp.Offer)
               .Include(tp => tp.Program)
               .ToListAsync();
        }

        public async Task<OfferProgram?> GetOfferProgramByIdAsync(int offerProgramId)
        {
            return await _context.OfferPrograms
                .Include(tp => tp.Offer)
                .Include(tp => tp.Program)
                .FirstOrDefaultAsync(tp => tp.OfferProgramId == offerProgramId);
        }
        public async Task<IEnumerable<OfferProgram>> GetOfferProgramsByOfferIdAsync(int offerId)
        {
            return await _context.OfferPrograms
                .Include(tp => tp.Offer)
                .Include(tp => tp.Program)
                .Where(tp => tp.OfferId == offerId)
                .OrderBy(tp => tp.DayNumber)
                .ToListAsync();
        }
        public async Task<IEnumerable<OfferProgram>> GetOfferProgramsByProgramIdAsync(int programId)
        {
            return await _context.OfferPrograms
                .Include(tp => tp.Offer)
                .Include(tp => tp.Program)
                .Where(tp => tp.ProgramId == programId)
                .ToListAsync();
        }
        public async Task<OfferProgram> CreateOfferProgramAsync(OfferProgram offerProgram)
        {
            offerProgram.CreatedAt = DateTime.Now;
            offerProgram.UpdatedAt = DateTime.Now;

            _context.OfferPrograms.Add(offerProgram);
            await _context.SaveChangesAsync();

            return offerProgram;
        }

        public async Task<OfferProgram?> UpdateOfferProgramAsync(int offerProgramId, OfferProgram offerProgram)
        {
            var existingOfferProgram = await _context.OfferPrograms.FindAsync(offerProgramId);

            if (existingOfferProgram == null)
            {
                return null;
            }

            existingOfferProgram.DayNumber = offerProgram.DayNumber;
            existingOfferProgram.ProgramDate = offerProgram.ProgramDate;
            existingOfferProgram.CustomTitle = offerProgram.CustomTitle;
            existingOfferProgram.CustomDescription = offerProgram.CustomDescription;
            existingOfferProgram.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return existingOfferProgram;
        }

        public async Task<bool> DeleteOfferProgramAsync(int offerProgramId)
        {
            var offerProgram = await _context.OfferPrograms.FindAsync(offerProgramId);

            if (offerProgram == null)
            {
                return false;
            }

            _context.OfferPrograms.Remove(offerProgram);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> OfferProgramExistsAsync(int offerProgramId)
        {
            return await _context.OfferPrograms.AnyAsync(tp => tp.OfferProgramId == offerProgramId);
        }

    }
}
