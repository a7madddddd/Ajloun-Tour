using Ajloun_Tour.DTOs2.ToursProgramDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;

namespace Ajloun_Tour.Implementations
{
    public class TourProgramServiceRepository : ITourProgramServiceRepository
    {

        private readonly IToursProgramRepository _tourProgramRepository;
        private readonly MyDbContext _context;

        public TourProgramServiceRepository(IToursProgramRepository tourProgramRepository, MyDbContext context)
        {
            _tourProgramRepository = tourProgramRepository;
            _context = context;
        }

        public async Task<IEnumerable<ToursProgramDTO>> GetAllTourProgramsAsync()
        {
            var tourPrograms = await _tourProgramRepository.GetAllTourProgramsAsync();
            return tourPrograms.Select(tp => MapToDTO(tp));
        }

        public async Task<ToursProgramDTO?> GetTourProgramByIdAsync(int tourProgramId)
        {
            var tourProgram = await _tourProgramRepository.GetTourProgramByIdAsync(tourProgramId);
            return tourProgram != null ? MapToDTO(tourProgram) : null;
        }

        public async Task<IEnumerable<ToursProgramDTO>> GetTourProgramsByTourIdAsync(int tourId)
        {
            var tourPrograms = await _tourProgramRepository.GetTourProgramsByTourIdAsync(tourId);
            return tourPrograms.Select(tp => MapToDTO(tp));
        }

        public async Task<TourWithProgramsDTO?> GetTourWithProgramsAsync(int tourId)
        {
            
            var tour = await _context.Tours.FindAsync(tourId);
            if (tour == null)
            {
                return null;
            }

            var tourPrograms = await _tourProgramRepository.GetTourProgramsByTourIdAsync(tourId);

            return new TourWithProgramsDTO
            {
                TourId = tour.TourId,
                TourName = tour.TourName,
                Programs = tourPrograms.Select(tp => MapToDTO(tp)).ToList()
            };
        }

        public async Task<ToursProgramDTO> CreateTourProgramAsync(CreateToursProgram createTourProgramDTO)
        {
            var tourProgram = new TourProgram
            {
                TourId = createTourProgramDTO.TourId,
                ProgramId = createTourProgramDTO.ProgramId,
                DayNumber = createTourProgramDTO.DayNumber,
                ProgramDate = createTourProgramDTO.ProgramDate,
                CustomTitle = createTourProgramDTO.CustomTitle,
                CustomDescription = createTourProgramDTO.CustomDescription
            };

            var createdTourProgram = await _tourProgramRepository.CreateTourProgramAsync(tourProgram);

            // Fetch the complete entity with related data for proper mapping
            var completeEntity = await _tourProgramRepository.GetTourProgramByIdAsync(createdTourProgram.TourProgramId);
            return MapToDTO(completeEntity!);
        }

        public async Task<ToursProgramDTO?> UpdateTourProgramAsync(int tourProgramId, UpdateTourProgramDTO updateTourProgramDTO)
        {
            var existingTourProgram = await _tourProgramRepository.GetTourProgramByIdAsync(tourProgramId);

            if (existingTourProgram == null)
            {
                return null;
            }

            existingTourProgram.DayNumber = updateTourProgramDTO.DayNumber;
            existingTourProgram.ProgramDate = updateTourProgramDTO.ProgramDate;
            existingTourProgram.CustomTitle = updateTourProgramDTO.CustomTitle;
            existingTourProgram.CustomDescription = updateTourProgramDTO.CustomDescription;

            var updatedTourProgram = await _tourProgramRepository.UpdateTourProgramAsync(
                tourProgramId, existingTourProgram);

            if (updatedTourProgram == null)
            {
                return null;
            }

            // Fetch the complete entity with related data for proper mapping
            var completeEntity = await _tourProgramRepository.GetTourProgramByIdAsync(tourProgramId);

            return MapToDTO(completeEntity!);
        }

        public async Task<bool> DeleteTourProgramAsync(int tourProgramId)
        {
            return await _tourProgramRepository.DeleteTourProgramAsync(tourProgramId);
        }

        private ToursProgramDTO MapToDTO(TourProgram tourProgram)
        {
            return new ToursProgramDTO
            {
                TourProgramId = tourProgram.TourProgramId,
                TourId = tourProgram.TourId,
                TourName = tourProgram.Tour?.TourName,
                ProgramId = tourProgram.ProgramId,
                ProgramTitle = tourProgram.Program?.Title,
                DayNumber = tourProgram.DayNumber,
                ProgramDate = tourProgram.ProgramDate,
                CustomTitle = tourProgram.CustomTitle,
                CustomDescription = tourProgram.CustomDescription
            };
        }
    }
}

