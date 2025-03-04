using Ajloun_Tour.DTOs2.PackagesProgramDTOs;
using Ajloun_Tour.DTOs2.ToursProgramDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;

namespace Ajloun_Tour.Implementations
{
    public class PackProgramServiceRepository : IPackProgramServiceRepository
    {
        private readonly MyDbContext _context;
        private readonly IPackageProgramRepository _packageProgramRepository;
        public PackProgramServiceRepository(IPackageProgramRepository packageProgramRepository, MyDbContext context)
        {
            _packageProgramRepository = packageProgramRepository;
            _context = context;
        }

        public async Task<IEnumerable<PackagesProgramDTO>> GetAllPackProgramsAsync()
        {
            var packPrograms = await _packageProgramRepository.GetAllPackProgramsAsync();
            return packPrograms.Select(tp => MapToDTO(tp));
        }
        public async Task<PackagesProgramDTO?> GetPackProgramByIdAsync(int packprogId)
        {
            var packProgram = await _packageProgramRepository.GetPackProgramByIdAsync(packprogId);
            return packProgram != null ? MapToDTO(packProgram) : null;
        }
        public async Task<IEnumerable<PackagesProgramDTO>> GetPackProgramsByPackIdAsync(int packId)
        {
            var packPrograms = await _packageProgramRepository.GetPackProgramsByPackIdAsync(packId);
            return packPrograms.Select(tp => MapToDTO(tp));
        }

        public async Task<PackageWithProgramsDTO?> GetPackWithProgramsAsync(int packId)
        {
            var pack = await _context.Packages.FindAsync(packId);
            if (pack == null)
            {
                return null;
            }

            var packPrograms = await _packageProgramRepository.GetPackProgramsByPackIdAsync(packId);

            return new PackageWithProgramsDTO
            {
                PackageId = pack.Id,
                Name = pack.Name,
                Programs = packPrograms.Select(tp => MapToDTO(tp)).ToList()
            };
        }

        public async Task<PackagesProgramDTO> CreatePackProgramAsync(CreatePackageProgram createPackageProgram)
        {
            var packProgram = new PackageProgram
            {
                PackageId = createPackageProgram.PackageId,
                ProgramId = createPackageProgram.ProgramId,
                DayNumber = createPackageProgram.DayNumber,
                ProgramDate = createPackageProgram.ProgramDate,
                CustomTitle = createPackageProgram.CustomTitle,
                CustomDescription = createPackageProgram.CustomDescription
            };

            var createdpackProgram = await _packageProgramRepository.CreatePackProgramAsync(packProgram);

            // Fetch the complete entity with related data for proper mapping
            var completeEntity = await _packageProgramRepository.GetPackProgramByIdAsync(createdpackProgram.PackageProgramId);
            return MapToDTO(completeEntity!);
        }
        public async Task<PackagesProgramDTO?> UpdatePackProgramAsync(int packProgId, UpdatePackageProgramDTO updatePackageProgramDTO)
        {
            var existingpackProgram = await _packageProgramRepository.GetPackProgramByIdAsync(packProgId);

            if (existingpackProgram == null)
            {
                return null;
            }

            existingpackProgram.DayNumber = updatePackageProgramDTO.DayNumber;
            existingpackProgram.ProgramDate = updatePackageProgramDTO.ProgramDate;
            existingpackProgram.CustomTitle = updatePackageProgramDTO.CustomTitle;
            existingpackProgram.CustomDescription = updatePackageProgramDTO.CustomDescription;

            var updatedTourProgram = await _packageProgramRepository.UpdatePackProgramAsync(
                packProgId, existingpackProgram);

            if (updatedTourProgram == null)
            {
                return null;
            }

            // Fetch the complete entity with related data for proper mapping
            var completeEntity = await _packageProgramRepository.GetPackProgramByIdAsync(packProgId);

            return MapToDTO(completeEntity!);
        }

        public async Task<bool> DeletePackProgramAsync(int packProId)
        {
            return await _packageProgramRepository.DeletePackProgramAsync(packProId);
        }

        private PackagesProgramDTO MapToDTO(PackageProgram packageProgram)
        {
            return new PackagesProgramDTO
            {
                PackageProgramId = packageProgram.PackageProgramId,
                PackageId = packageProgram.PackageId,
                Name = packageProgram.Package.Name,
                ProgramId = packageProgram.ProgramId,
                ProgramTitle = packageProgram.Program?.Title,
                DayNumber = packageProgram.DayNumber,
                ProgramDate = packageProgram.ProgramDate,
                CustomTitle = packageProgram.CustomTitle,
                CustomDescription = packageProgram.CustomDescription,
                CreatedAt = packageProgram.CreatedAt,
                UpdatedAt = packageProgram.UpdatedAt,
            };
        }

    }
}
