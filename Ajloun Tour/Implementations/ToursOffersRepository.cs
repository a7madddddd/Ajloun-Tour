using Ajloun_Tour.DTOs.ToursOffersDTOs;
using Ajloun_Tour.DTOs2.TourCartDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class ToursOffersRepository : IToursOffersRepository
    {

        private readonly MyDbContext _context;

        public ToursOffersRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ToursOffersDTO>> GetAllToursOffers()
        {
            return await _context.TourOffers
              .Include(to => to.Tour)
              .Include(to => to.Offer)
              .Select(to => new ToursOffersDTO
              {
                  TourId = to.TourId,
                  OfferId = to.OfferId,
                  TourName = to.Tour.TourName,
                  OfferTitle = to.Offer.Title,
                  Image = to.Offer.Image,
                  DiscountPercentage = to.Offer.DiscountPercentage ?? 0,
                  IsActive = to.Offer.IsActive,
                  Price = to.Offer.Price,
                  Peapole = to.Offer.Peapole,
                  Description = to.Offer.Description,
                  StartDate = to.Offer.StartDate ?? DateTime.MinValue,
                  EndDate = to.Offer.EndDate ?? DateTime.MinValue
              })
              .ToListAsync();
        }
        public async Task<ToursOffersDTO> GetTourOfferById(int tourId, int offerId)
        {
            return await _context.TourOffers
                       .Include(to => to.Tour)
                       .Include(to => to.Offer)
                       .Where(to => to.TourId == tourId && to.OfferId == offerId)
                       .Select(to => new ToursOffersDTO
                       {
                           TourId = to.TourId,
                           OfferId = to.OfferId,
                           TourName = to.Tour.TourName,
                           Image = to.Offer.Image,
                           OfferTitle = to.Offer.Title,
                           Price  = to.Offer.Price,
                           Peapole = to.Offer.Peapole,
                           Description = to.Offer.Description,
                           IsActive = to.Offer.IsActive,
                           DiscountPercentage = (decimal)to.Offer.DiscountPercentage,
                           StartDate = (DateTime)to.Offer.StartDate,
                           EndDate = (DateTime)to.Offer.EndDate,
                           Map = to.Tour.Map
                       })
                       .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ToursOffersDTO>> GetActiveOffers()
        {
            var currentDate = DateTime.UtcNow.Date;
            return await _context.TourOffers
                .Include(to => to.Tour)
                .Include(to => to.Offer)
                .Where(to => to.Offer.StartDate <= currentDate && to.Offer.EndDate >= currentDate)
                .Select(to => new ToursOffersDTO
                {
                    TourId = to.TourId,
                    OfferId = to.OfferId,
                    TourName = to.Tour.TourName,
                    OfferTitle = to.Offer.Title,
                    Peapole = to.Offer.Peapole,
                    Description= to.Offer.Description,
                    Price = to.Offer.Price,
                    Image = to.Offer.Image,
                    DiscountPercentage = (decimal)to.Offer.DiscountPercentage,
                    StartDate = (DateTime)to.Offer.StartDate,
                    EndDate = (DateTime)to.Offer.EndDate
                })
                .ToListAsync();
        }

        public async Task<ToursOffersDTO> AddTourOffer(CreateToursOffer createToursOffer)
        {
            // Check if the Tour and Offer exist
            var tour = await _context.Tours.FirstOrDefaultAsync(t => t.TourId == createToursOffer.TourId);
            if (tour == null)
            {
                throw new InvalidOperationException($"Tour with ID {createToursOffer.TourId} does not exist.");
            }

            var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == createToursOffer.OfferId);
            if (offer == null)
            {
                throw new InvalidOperationException($"Offer with ID {createToursOffer.OfferId} does not exist.");
            }

            // Create the new TourOffer object
            var tourOffer = new TourOffer
            {
                TourId = createToursOffer.TourId,
                OfferId = createToursOffer.OfferId,
                Tour = tour,        // Link the Tour object
                Offer = offer,      // Link the Offer object
            };

            // Add the TourOffer to the context
            _context.TourOffers.Add(tourOffer);
            await _context.SaveChangesAsync();

            // Returning the created TourOffer as ToursOffersDTO
            return new ToursOffersDTO
            {
                TourId = tourOffer.TourId,
                OfferId = tourOffer.OfferId,
                TourName = tourOffer.Tour?.TourName, // Assuming TourName is a property of Tour
                OfferTitle = offer?.Title,           // Assuming Title is a property of Offer
                Price = offer.Price,
                Peapole = offer.Peapole,
                Description = offer.Description, 
                IsActive = offer.IsActive,
                DiscountPercentage = createToursOffer.DiscountPercentage,
                StartDate = createToursOffer.StartDate,
                EndDate = createToursOffer.EndDate
            };
        }


        public async Task<ToursOffersDTO> UpdateTourOffer(int tourId, CreateToursOffer createToursOffer)
        {
            var existingTourOffer = await _context.TourOffers
                .FirstOrDefaultAsync(to => to.TourId == tourId);

            if (existingTourOffer == null)
            {
                throw new KeyNotFoundException($"No tour offer found with tour ID: {tourId}");
            }

            var offerExists = await _context.Offers
                .AnyAsync(o => o.Id == createToursOffer.OfferId);

            if (!offerExists)
            {
                throw new InvalidOperationException($"Offer with ID {createToursOffer.OfferId} does not exist");
            }

            var duplicateExists = await _context.TourOffers
                .AnyAsync(to =>
                    to.TourId == createToursOffer.TourId &&
                    to.OfferId == createToursOffer.OfferId &&
                    to.TourId != tourId);

            if (duplicateExists)
            {
                throw new InvalidOperationException("This Tour-Offer relationship already exists");
            }

            existingTourOffer.OfferId = createToursOffer.OfferId;

            if (createToursOffer.TourId != tourId)
            {
                var newTourExists = await _context.Tours
                    .AnyAsync(t => t.TourId == createToursOffer.TourId);

                if (!newTourExists)
                {
                    throw new InvalidOperationException($"Tour with ID {createToursOffer.TourId} does not exist");
                }

                existingTourOffer.TourId = createToursOffer.TourId;
            }

            await _context.SaveChangesAsync();

            return new ToursOffersDTO
            {
                TourId = existingTourOffer.TourId,
                OfferId = existingTourOffer.OfferId
            };
        }




        public async Task<bool> DeleteTourOffer(int tourId, int offerId)
        {
            var tourOffer = await _context.TourOffers
                        .FirstOrDefaultAsync(to => to.TourId == tourId && to.OfferId == offerId);

            if (tourOffer == null)
                return false;

            _context.TourOffers.Remove(tourOffer);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
