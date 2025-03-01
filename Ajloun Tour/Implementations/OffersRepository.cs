using Ajloun_Tour.DTOs.NewsLattersDTO;
using Ajloun_Tour.DTOs.OffersDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class OffersRepository : IOffersRepository
    {
        private readonly MyDbContext _context;

        public OffersRepository(MyDbContext context)
        {

            _context = context;
        }
        public async Task<IEnumerable<OffersDTO>> GetAllOffersAsync()
        {
            var offers = await _context.Offers.ToListAsync();

            return offers.Select(o => new OffersDTO
            {

                Id = o.Id,
                Title = o.Title,
                DiscountPercentage = o.DiscountPercentage,
                StartDate = o.StartDate,
                EndDate = o.EndDate,
                IsActive = o.IsActive,
            });
        }

        public async Task<OffersDTO> GetOffersById(int id)
        {
            var offer = await _context.Offers.FindAsync(id);

            if (offer == null)
            {
                throw new Exception("This Offer Is Not Defined");
            }

            // Log the IsActive value for debugging
            Console.WriteLine($"IsActive: {offer.IsActive}");

            return new OffersDTO
            {
                Id = offer.Id,
                Title = offer.Title,
                DiscountPercentage = offer.DiscountPercentage,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                IsActive = offer.IsActive,
            };
        }

        public async Task<OffersDTO> AddOffersAsync(CreateOffers createOffers)
        {
            if (createOffers == null)
            {
                throw new ArgumentNullException(nameof(createOffers), "The offer data cannot be null.");
            }

            var offer = new Offer
            {
                Title = createOffers.Title,
                DiscountPercentage = createOffers.DiscountPercentage,
                StartDate = createOffers.StartDate,
                EndDate = createOffers.EndDate,
                IsActive = createOffers.IsActive,
            };

            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();

            return new OffersDTO
            {
                Id = offer.Id,
                Title = offer.Title,
                DiscountPercentage = offer.DiscountPercentage,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                IsActive = offer.IsActive,
            };
        }

        public async Task<OffersDTO> UpdateOffersAsync(int id, CreateOffers createOffers)
        {
            var updatedoffers = await _context.Offers.FindAsync(id);

            if (updatedoffers == null)
            {
                throw new KeyNotFoundException("This Offer is not defined.");
            }

            updatedoffers.Title = createOffers.Title ?? updatedoffers.Title;
            updatedoffers.DiscountPercentage = createOffers.DiscountPercentage ?? updatedoffers.DiscountPercentage;
            updatedoffers.StartDate = createOffers.StartDate ?? updatedoffers.StartDate;
            updatedoffers.EndDate = createOffers.EndDate ?? updatedoffers.EndDate;
            updatedoffers.IsActive = createOffers.IsActive ?? updatedoffers.IsActive;

            _context.Offers.Update(updatedoffers);
            await _context.SaveChangesAsync();

            return new OffersDTO
            {
                Id = updatedoffers.Id,
                Title = updatedoffers.Title,
                DiscountPercentage = updatedoffers.DiscountPercentage,
                StartDate = updatedoffers.StartDate,
                EndDate = updatedoffers.EndDate,
                IsActive = updatedoffers.IsActive,
            };
        }


        public async Task DeleteOffersAsync(int id)
        {
            var deleteOffer = await _context.Offers.FindAsync(id);

            if (deleteOffer == null)
            {

                throw new Exception("This Offer Is Not Defined");
            }

            _context.Offers.Remove(deleteOffer);
            await _context.SaveChangesAsync();
        }



    }
}
