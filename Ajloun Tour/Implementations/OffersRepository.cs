using Ajloun_Tour.DTOs.OffersDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class OffersRepository : IOffersRepository
    {
        private readonly MyDbContext _context;

        public OffersRepository(MyDbContext context) { 
        
            _context = context;
        }
        public async Task<IEnumerable<OffersDTO>> GetAllOffersAsync()
        {
            var offers = await _context.Offers.ToListAsync();

            return offers.Select(o => new OffersDTO {
            
                
            });
        }

        public Task<OffersDTO> AddOffersAsync(CreateOffers createOffers)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOffersAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<OffersDTO> GetOffersById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OffersDTO> UpdateOffersAsync(int id, CreateOffers createOffers)
        {
            throw new NotImplementedException();
        }
    }
}
