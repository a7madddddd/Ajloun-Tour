using Ajloun_Tour.DTOs2.ToursProgramDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface ITourProgramServiceRepository
    {
        Task<IEnumerable<ToursProgramDTO>> GetAllTourProgramsAsync();
        Task<ToursProgramDTO?> GetTourProgramByIdAsync(int tourProgramId);
        Task<IEnumerable<ToursProgramDTO>> GetTourProgramsByTourIdAsync(int tourId);
        Task<TourWithProgramsDTO?> GetTourWithProgramsAsync(int tourId);
        Task<ToursProgramDTO> CreateTourProgramAsync(CreateToursProgram createTourProgramDTO);
        Task<ToursProgramDTO?> UpdateTourProgramAsync(int tourProgramId, UpdateTourProgramDTO updateTourProgramDTO);
        Task<bool> DeleteTourProgramAsync(int tourProgramId);
    }
}
