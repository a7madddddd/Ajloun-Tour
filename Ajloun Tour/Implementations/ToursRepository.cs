using Ajloun_Tour.DTOs.ToursDTOs;
using Ajloun_Tour.DTOs.UsersDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class ToursRepository : IToursRepository
    {
        private readonly MyDbContext _context;
        private readonly string _imageDirectory;


        public ToursRepository(MyDbContext context)
        {
            _context = context;
            _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ToursImages");
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

        public async Task<IEnumerable<ToursDTO>> GetAllToursAsync()
        {
            var tours = await _context.Tours.ToListAsync();

            return tours.Select(t => new ToursDTO
            {

                TourId = t.TourId,
                TourName = t.TourName,
                Description = t.Description,
                Duration = t.Duration,
                Details = t.Details,
                IsActive = t.IsActive,
                Price = t.Price,
                TourImage = t.TourImage,
                Location = t.Location,
                Map = t.Map,

            });
        }

        public async Task<ToursDTO> GetToursByIdAsync(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {

                throw new Exception("This Tour Is Not Defined");
            }

            return new ToursDTO
            {

                TourId = tour.TourId,
                TourName = tour.TourName,
                Description = tour.Description,
                Duration = tour.Duration,
                Details = tour.Details,
                IsActive = tour.IsActive,
                Price = tour.Price,
                TourImage = tour.TourImage,
                Location= tour.Location,
                Map = tour.Map,

            };
        }

        public async Task<ToursDTO> AddToursAsync(CreateTours createTours)
        {

            if (createTours.TourImage == null)
            {
                throw new ArgumentNullException(nameof(createTours.TourImage), "Image file is required.");
            }

            var fileName = await SaveImageFileAsync(createTours.TourImage);

            var tour = new Tour
            {

                TourName = createTours.TourName,
                Description = createTours.Description,
                Duration = createTours.Duration,
                Details = createTours.Details,
                IsActive = createTours.IsActive,
                Price = (decimal)createTours.Price,
                Location = createTours.Location,
                Map = createTours.Map,
                TourImage = fileName
            };


            await _context.Tours.AddAsync(tour);
            await _context.SaveChangesAsync();

            return new ToursDTO
            {

                TourId = tour.TourId,
                TourName = tour.TourName,
                Description = tour.Description,
                Duration = tour.Duration,
                Details = tour.Details,
                IsActive = tour.IsActive,
                Price = tour.Price,
                Location = tour.Location,
                Map = tour.Map,
                TourImage = fileName

            };




        }

        public async Task<ToursDTO> UpdateToursAsync(int id, CreateTours createTours)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                throw new KeyNotFoundException($"Tour with ID {id} not found.");
            }

            // Update fields if a value is provided, otherwise retain the existing value
            if (!string.IsNullOrEmpty(createTours.TourName))
            {
                tour.TourName = createTours.TourName;
            }

            if (!string.IsNullOrEmpty(createTours.Description))
            {
                tour.Description = createTours.Description;
            }

            if (createTours.Price.HasValue)
            {
                tour.Price = createTours.Price.Value;
            }

            if (!string.IsNullOrEmpty(createTours.Duration))
            {
                tour.Duration = createTours.Duration;
            }

            if (!string.IsNullOrEmpty(createTours.Details))
            {
                tour.Details = createTours.Details;
            }

            if (!string.IsNullOrEmpty(createTours.Location))
            {
                tour.Location = createTours.Location;
            }
            if (!string.IsNullOrEmpty(createTours.Map))
            {
                tour.Map = createTours.Map;
            }

            // Handle TourImage file upload only if provided
            if (createTours.TourImage != null)
            {
                // If there was an existing image, delete it before uploading the new one
                if (!string.IsNullOrEmpty(tour.TourImage))
                {
                    await DeleteImageFileAsync(tour.TourImage);
                }

                // Save new image and update the TourImage path
                var fileName = await SaveImageFileAsync(createTours.TourImage);
                tour.TourImage = fileName;
            }

            // Update IsActive if a value is provided, otherwise retain the existing value
            if (createTours.IsActive.HasValue)
            {
                tour.IsActive = createTours.IsActive.Value;
            }

            // Save changes to the database
            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();

            return new ToursDTO
            {
                TourId = tour.TourId,
                TourName = tour.TourName,
                Description = tour.Description,
                Duration = tour.Duration,
                Details = tour.Details,
                Price = tour.Price,
                IsActive = tour.IsActive,
                Location = tour.Location,
                Map = tour.Map,
                TourImage = tour.TourImage,
            };
        }

        public async Task DeleteToursAsync(int id)
        {
            var DeleteTour = await _context.Tours.FindAsync(id);

            if (DeleteTour == null) {

                throw new Exception("This Tour Is Not Defined");
            }

            _context.Tours.Remove(DeleteTour);
            await _context.SaveChangesAsync();
        }
    }
}
