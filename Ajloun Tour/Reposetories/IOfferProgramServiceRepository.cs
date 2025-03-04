using Ajloun_Tour.DTOs2.OffersProgramDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IOfferProgramServiceRepository
    {
        Task<IEnumerable<OffersProgramDTO>> GetAllOfferProgramsAsync();
        Task<OffersProgramDTO?> GetOfferProgramByIdAsync(int offerProgramId);
        Task<IEnumerable<OffersProgramDTO>> GetOfferProgramsByOfferIdAsync(int offerId);
        Task<OfferWithProgramsDTO?> GetOfferWithProgramsAsync(int offerId);
        Task<OffersProgramDTO> CreateOfferProgramAsync(CreateOffersProgram createOffersProgram);
        Task<OffersProgramDTO?> UpdateOfferProgramAsync(int offerProgramId, UpdateOfferProgramDTO updateOfferProgramDTO);
        Task<bool> DeleteOfferProgramAsync(int offerProgramId);
    }
}
