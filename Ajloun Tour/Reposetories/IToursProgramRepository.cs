using Ajloun_Tour.DTOs2;
using Ajloun_Tour.DTOs2.PackagesProgramDTOs;
using Ajloun_Tour.DTOs2.ToursProgramDTOs;
using Ajloun_Tour.Models;

namespace Ajloun_Tour.Reposetories
{
    public interface IToursProgramRepository
    {
        Task<IEnumerable<TourProgram>> GetAllTourProgramsAsync();
        Task<TourProgram?> GetTourProgramByIdAsync(int tourProgramId);
        Task<IEnumerable<TourProgram>> GetTourProgramsByTourIdAsync(int tourId);
        Task<IEnumerable<TourProgram>> GetTourProgramsByProgramIdAsync(int programId);
        Task<TourProgram> CreateTourProgramAsync(TourProgram tourProgram);
        Task<TourProgram?> UpdateTourProgramAsync(int tourProgramId, TourProgram tourProgram);
        Task<bool> DeleteTourProgramAsync(int tourProgramId);
        Task<bool> TourProgramExistsAsync(int tourProgramId);
    }
}
