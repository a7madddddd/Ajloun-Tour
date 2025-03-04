using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class PackageProgramRepository : IPackageProgramRepository
    {
        private readonly MyDbContext _context;

        public PackageProgramRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PackageProgram>> GetAllPackProgramsAsync()
        {
            return await _context.PackagePrograms
            .Include(tp => tp.Package)
            .Include(tp => tp.Program)
            .ToListAsync();
        }
        public async Task<PackageProgram?> GetPackProgramByIdAsync(int packProgId)
        {
            return await _context.PackagePrograms
                .Include(tp => tp.Package)
                .Include(tp => tp.Program)
                .FirstOrDefaultAsync(tp => tp.PackageProgramId == packProgId);
        }
        public async Task<IEnumerable<PackageProgram>> GetPackProgramsByPackIdAsync(int packId)
        {
            return await _context.PackagePrograms
                .Include(tp => tp.Package)
                .Include(tp => tp.Program)
                .Where(tp => tp.PackageId == packId)
                .OrderBy(tp => tp.DayNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<PackageProgram>> GetPackProgramsByProgramIdAsync(int programId)
        {
            return await _context.PackagePrograms
                .Include(tp => tp.Package)
                .Include(tp => tp.Program)
                .Where(tp => tp.ProgramId == programId)
                .ToListAsync();
        }

        public async Task<PackageProgram> CreatePackProgramAsync(PackageProgram packageProgram)
        {
            packageProgram.CreatedAt = DateTime.Now;
            packageProgram.UpdatedAt = DateTime.Now;

            _context.PackagePrograms.Add(packageProgram);
            await _context.SaveChangesAsync();

            return packageProgram;
        }


        public async Task<PackageProgram?> UpdatePackProgramAsync(int packProgramId, PackageProgram packageProgram)
        {
            var existingPackProgram = await _context.PackagePrograms.FindAsync(packProgramId);

            if (existingPackProgram == null)
            {
                return null;
            }

            existingPackProgram.DayNumber = packageProgram.DayNumber;
            existingPackProgram.ProgramDate = packageProgram.ProgramDate;
            existingPackProgram.CustomTitle = packageProgram.CustomTitle;
            existingPackProgram.CustomDescription = packageProgram.CustomDescription;
            existingPackProgram.CreatedAt = DateTime.Now;
            existingPackProgram.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return existingPackProgram;
        }

        public async Task<bool> DeletePackProgramAsync(int packProgramId)
        {
            var packProgram = await _context.PackagePrograms.FindAsync(packProgramId);

            if (packProgram == null)
            {
                return false;
            }

            _context.PackagePrograms.Remove(packProgram);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> PackProgramExistsAsync(int packProgramId)
        {
            return await _context.PackagePrograms.AnyAsync(tp => tp.PackageProgramId == packProgramId);
        }

    }
}
