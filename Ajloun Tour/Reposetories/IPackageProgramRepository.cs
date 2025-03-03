using Ajloun_Tour.Models;

namespace Ajloun_Tour.Reposetories
{
    public interface IPackageProgramRepository
    {
        Task<IEnumerable<PackageProgram>> GetAllPackProgramsAsync();
        Task<PackageProgram?> GetPackProgramByIdAsync(int packProgId);
        Task<IEnumerable<PackageProgram>> GetPackProgramsByPackIdAsync(int packId);
        Task<IEnumerable<PackageProgram>> GetPackProgramsByProgramIdAsync(int programId);
        Task<PackageProgram> CreatePackProgramAsync(PackageProgram packageProgram);
        Task<PackageProgram?> UpdatePackProgramAsync(int packProgId, PackageProgram packageProgram);
        Task<bool> DeletePackProgramAsync(int packProgramId);
        Task<bool> PackProgramExistsAsync(int packProgramId);
    }
}
