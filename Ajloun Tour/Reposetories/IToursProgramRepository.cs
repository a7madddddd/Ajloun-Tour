using Ajloun_Tour.DTOs2.PackagesProgramDTOs;
using Ajloun_Tour.DTOs2.ToursProgramDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IToursProgramRepository
    {
        Task<IEnumerable<ToursProgramDTO>> GetToursPrograms();

        Task<ToursProgramDTO> GetTourProgram(int id);

        Task<ToursProgramDTO> GetProgramByTourId(int tourId);
        Task<PackageWithProgramsDTO?> GetPackageWithProgramsAsync(int packageId);
        Task<ToursProgramDTO> AddToursProgram(CreateToursProgram createToursProgram);

        Task<ToursProgramDTO> UpdateToursProgram(int id, CreateToursProgram createToursProgram);

        Task DeleteToursProgram(int id);
    }
}
