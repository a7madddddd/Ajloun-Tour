using Ajloun_Tour.DTOs.ToursOffersDTOs;
using Ajloun_Tour.Models;

namespace Ajloun_Tour.Reposetories
{
    public interface IToursOffersRepository
    {
        Task<IEnumerable<ToursOffersDTO>> GetAllToursOffers();

        Task<ToursOffersDTO> GetTourOfferById(int tourId, int offerId);

        Task<ToursOffersDTO> AddTourOffer(CreateToursOffer createToursOffer);

        Task<ToursOffersDTO> UpdateTourOffer(int tourId, CreateToursOffer createToursOffer);

        Task<bool> DeleteTourOffer(int tourId, int offerId);

        Task<IEnumerable<ToursOffersDTO>> GetActiveOffers();

    }
}
