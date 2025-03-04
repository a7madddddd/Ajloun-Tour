using Ajloun_Tour.DTOs2.OffersProgramDTOs;
using Ajloun_Tour.DTOs2.ToursProgramDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;

namespace Ajloun_Tour.Implementations
{
    public class OfferProgramServiceRepository : IOfferProgramServiceRepository
    {
        private readonly IOffersProgramRepository _offersProgramRepository;
        private readonly MyDbContext _context;

        public OfferProgramServiceRepository(IOffersProgramRepository offersProgramRepository, MyDbContext context)
        {
            _offersProgramRepository = offersProgramRepository;
            _context = context;
        }

        public async Task<OffersProgramDTO> CreateOfferProgramAsync(CreateOffersProgram createOffersProgram)
        {
            var offerProgram = new OfferProgram
            {
                OfferId = createOffersProgram.OfferId,
                ProgramId = createOffersProgram.ProgramId,
                DayNumber = createOffersProgram.DayNumber,
                ProgramDate = createOffersProgram.ProgramDate,
                CustomTitle = createOffersProgram.CustomTitle,
                CustomDescription = createOffersProgram.CustomDescription
            };

            var createdOfferProgram = await _offersProgramRepository.CreateOfferProgramAsync(offerProgram);

            // Fetch the complete entity with related data for proper mapping
            var completeEntity = await _offersProgramRepository.GetOfferProgramByIdAsync(createdOfferProgram.OfferProgramId);
            return MapToDTO(completeEntity!);
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

        public async Task<IEnumerable<OffersProgramDTO>> GetAllOfferProgramsAsync()
        {
            var offerPrograms = await _offersProgramRepository.GetAllOfferProgramsAsync();
            return offerPrograms.Select(tp => MapToDTO(tp));
        }

        public async Task<OffersProgramDTO?> GetOfferProgramByIdAsync(int offerProgramId)
        {
            var offerProgram = await _offersProgramRepository.GetOfferProgramByIdAsync(offerProgramId);
            return offerProgram != null ? MapToDTO(offerProgram) : null;
        }

        public async Task<IEnumerable<OffersProgramDTO>> GetOfferProgramsByOfferIdAsync(int offerId)
        {
            var offerPrograms = await _offersProgramRepository.GetOfferProgramsByOfferIdAsync(offerId);
            return offerPrograms.Select(tp => MapToDTO(tp));
        }

        public async Task<OfferWithProgramsDTO?> GetOfferWithProgramsAsync(int offerId)
        {
            var offer = await _context.Offers.FindAsync(offerId);
            if (offer == null)
            {
                return null;
            }

            var offerPrograms = await _offersProgramRepository.GetOfferProgramsByOfferIdAsync(offerId);

            return new OfferWithProgramsDTO
            {
                OfferId = offer.Id,
                Title = offer.Title,
                Programs = offerPrograms.Select(tp => MapToDTO(tp)).ToList()
            };
        }

        public async Task<OffersProgramDTO?> UpdateOfferProgramAsync(int offerProgramId, UpdateOfferProgramDTO updateOfferProgramDTO)
        {
            var existingOfferProgram = await _offersProgramRepository.GetOfferProgramByIdAsync(offerProgramId);

            if (existingOfferProgram == null)
            {
                return null;
            }

            existingOfferProgram.DayNumber = updateOfferProgramDTO.DayNumber;
            existingOfferProgram.ProgramDate = updateOfferProgramDTO.ProgramDate;
            existingOfferProgram.CustomTitle = updateOfferProgramDTO.CustomTitle;
            existingOfferProgram.CustomDescription = updateOfferProgramDTO.CustomDescription;

            var updatedOfferProgram = await _offersProgramRepository.UpdateOfferProgramAsync(
                offerProgramId, existingOfferProgram);

            if (updatedOfferProgram == null)
            {
                return null;
            }

            // Fetch the complete entity with related data for proper mapping
            var completeEntity = await _offersProgramRepository.GetOfferProgramByIdAsync(offerProgramId);

            return MapToDTO(completeEntity!);
        }

        private OffersProgramDTO MapToDTO(OfferProgram offerProgram)
        {
            return new OffersProgramDTO
            {
                OfferProgramId = offerProgram.OfferProgramId,
                OfferId = offerProgram.OfferId,
                Title = offerProgram.Offer?.Title,
                ProgramId = offerProgram.ProgramId,
                ProgramTitle = offerProgram.Program?.Title,
                DayNumber = offerProgram.DayNumber,
                ProgramDate = offerProgram.ProgramDate,
                CustomTitle = offerProgram.CustomTitle,
                CustomDescription = offerProgram.CustomDescription
            };
        }
    }
}
