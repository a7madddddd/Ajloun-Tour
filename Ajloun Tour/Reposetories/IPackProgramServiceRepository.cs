using Ajloun_Tour.DTOs2.PackagesProgramDTOs;
using Ajloun_Tour.DTOs2.ToursProgramDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IPackProgramServiceRepository
    {
        Task<IEnumerable<PackagesProgramDTO>> GetAllPackProgramsAsync();
        Task<PackagesProgramDTO?> GetPackProgramByIdAsync(int packprogId);
        Task<IEnumerable<PackagesProgramDTO>> GetPackProgramsByPackIdAsync(int packId);
        Task<PackageWithProgramsDTO?> GetPackWithProgramsAsync(int packId);
        Task<PackagesProgramDTO> CreatePackProgramAsync(CreatePackageProgram createPackageProgram);
        Task<PackagesProgramDTO?> UpdatePackProgramAsync(int packprogrId, UpdatePackageProgramDTO updatePackageProgramDTO);
        Task<bool> DeletePackProgramAsync(int packProId);
    }
}
