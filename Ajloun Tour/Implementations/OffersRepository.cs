using Ajloun_Tour.DTOs.NewsLattersDTO;
using Ajloun_Tour.DTOs.OffersDTOs;
using Ajloun_Tour.DTOs.ToursDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class OffersRepository : IOffersRepository
    {
        private readonly MyDbContext _context;
        private readonly string _imageDirectory;


        public OffersRepository(MyDbContext context)
        {

            _context = context;
            _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OffersImages");
            EnsureImageDirectoryExists();
        }


        private void EnsureImageDirectoryExists()
        {
            if (!Directory.Exists(_imageDirectory))
            {
                Directory.CreateDirectory(_imageDirectory);
            }
        }

        private async Task<string> SaveImageFileAsync(IFormFile imageFile)
        {
            string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            string filePath = Path.Combine(_imageDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return fileName;
        }


        private async Task DeleteImageFileAsync(string fileName)
        {
            string filePath = Path.Combine(_imageDirectory, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
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
                Price = o.Price,
                Peapole = o.Peapole,
                Image = o.Image,
                Description = o.Description,
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
                Price = offer.Price,
                Peapole = offer.Peapole,
                Description = offer.Description,
                Image = offer.Image,
            };
        }

        public async Task<OffersDTO> AddOffersAsync(CreateOffers createOffers)
        {
            if (createOffers == null)
            {
                throw new ArgumentNullException(nameof(createOffers), "The offer data cannot be null.");
            }

            // Validate dates
            if (createOffers.StartDate.HasValue && createOffers.EndDate.HasValue && createOffers.StartDate > createOffers.EndDate)
            {
                throw new ArgumentException("Start date cannot be later than the end date.");
            }

            // Validate discount percentage
            if (createOffers.DiscountPercentage.HasValue && (createOffers.DiscountPercentage < 0 || createOffers.DiscountPercentage > 100))
            {
                throw new ArgumentException("Discount percentage must be between 0 and 100.");
            }

            // Save the image file
            var fileName = await SaveImageFileAsync(createOffers.Image);

            // Create the offer entity
            var offer = new Offer
            {
                Title = createOffers.Title,
                DiscountPercentage = createOffers.DiscountPercentage,
                StartDate = createOffers.StartDate,
                EndDate = createOffers.EndDate,
                IsActive = createOffers.IsActive ?? false, // Ensure default to false if null
                Price   = createOffers.Price,
                Peapole = createOffers.Peapole,
                Description = createOffers.Description,
                Image = fileName,
            };

            // Add to the database
            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();

            // Return the DTO
            return new OffersDTO
            {
                Id = offer.Id,
                Title = offer.Title,
                DiscountPercentage = offer.DiscountPercentage,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                IsActive = offer.IsActive,
                Price = offer.Price,
                Peapole = offer.Peapole,
                Description= offer.Description,
                Image = fileName,
            };
        }

        public async Task<OffersDTO> UpdateOffersAsync(int id, CreateOffers createOffers)
        {
            var updatedOffers = await _context.Offers.FindAsync(id);

            if (updatedOffers == null)
            {
                throw new KeyNotFoundException("This Offer is not defined.");
            }

            // Validate date consistency
            if (createOffers.StartDate.HasValue && createOffers.EndDate.HasValue && createOffers.StartDate > createOffers.EndDate)
            {
                throw new ArgumentException("Start date cannot be later than end date.");
            }

            // Validate discount percentage range (0-100)
            if (createOffers.DiscountPercentage.HasValue && (createOffers.DiscountPercentage < 0 || createOffers.DiscountPercentage > 100))
            {
                throw new ArgumentException("Discount percentage must be between 0 and 100.");
            }

            // Update properties, only if new values are provided
            updatedOffers.Title = createOffers.Title ?? updatedOffers.Title;
            updatedOffers.DiscountPercentage = createOffers.DiscountPercentage ?? updatedOffers.DiscountPercentage;
            updatedOffers.StartDate = createOffers.StartDate ?? updatedOffers.StartDate;
            updatedOffers.EndDate = createOffers.EndDate ?? updatedOffers.EndDate;
            updatedOffers.Price = createOffers.Price ?? updatedOffers.Price;
            updatedOffers.Peapole = createOffers.Peapole ?? updatedOffers.Peapole;
            updatedOffers.Description = createOffers.Description ?? updatedOffers.Description;
            updatedOffers.IsActive = createOffers.IsActive ?? updatedOffers.IsActive;

            // Handle image update if provided
            if (createOffers.Image != null)
            {
                if (!string.IsNullOrEmpty(updatedOffers.Image))
                {
                    await DeleteImageFileAsync(updatedOffers.Image);
                }

                var fileName = await SaveImageFileAsync(createOffers.Image);
                updatedOffers.Image = fileName;
            }

            // Update the entity in the database
            _context.Offers.Update(updatedOffers);
            await _context.SaveChangesAsync();

            // Return the updated Offer DTO
            return new OffersDTO
            {
                Id = updatedOffers.Id,
                Title = updatedOffers.Title,
                DiscountPercentage = updatedOffers.DiscountPercentage,
                StartDate = updatedOffers.StartDate,
                EndDate = updatedOffers.EndDate,
                IsActive = updatedOffers.IsActive,
                Price = updatedOffers.Price,
                Peapole = updatedOffers.Peapole,
                Description = updatedOffers.Description,
                Image = updatedOffers.Image,
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
