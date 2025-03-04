using Ajloun_Tour.Models;

namespace Ajloun_Tour.Reposetories
{
    public interface IOffersProgramRepository
    {
        Task<IEnumerable<OfferProgram>> GetAllOfferProgramsAsync();
        Task<OfferProgram?> GetOfferProgramByIdAsync(int offerProgramId);
        Task<IEnumerable<OfferProgram>> GetOfferProgramsByOfferIdAsync(int offerId);
        Task<IEnumerable<OfferProgram>> GetOfferProgramsByProgramIdAsync(int programId);
        Task<OfferProgram> CreateOfferProgramAsync(OfferProgram offerProgram);
        Task<OfferProgram?> UpdateOfferProgramAsync(int offerProgramId, OfferProgram offerProgram);
        Task<bool> DeleteOfferProgramAsync(int offerProgramId);
        Task<bool> OfferProgramExistsAsync(int offerProgramId);
    }
}
