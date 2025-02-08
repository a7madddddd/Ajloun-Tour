using Ajloun_Tour.DTOs.ToursDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IToursRepository
    {
        Task<IEnumerable<ToursDTO>> GetAllToursAsync();

        Task<ToursDTO> GetToursByIdAsync(int id);

        Task<ToursDTO> AddToursAsync(CreateTours createTours);

        Task<ToursDTO> UpdateToursAsync(int id, CreateTours createTours);

        Task DeleteToursAsync(int id);
    }
}
