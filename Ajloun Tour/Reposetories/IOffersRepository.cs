using Ajloun_Tour.DTOs.OffersDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IOffersRepository
    {
        Task<IEnumerable<OffersDTO>> GetAllOffersAsync();

        Task<OffersDTO> GetOffersById(int id);

        Task<OffersDTO> AddOffersAsync(CreateOffers createOffers);

        Task<OffersDTO> UpdateOffersAsync(int id, CreateOffers createOffers);

        Task DeleteOffersAsync(int id);
    }
}
