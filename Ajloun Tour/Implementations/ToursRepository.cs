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
                IsActive = t.IsActive,
                Price = t.Price,
                TourImage = t.TourImage,

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
                IsActive = tour.IsActive,
                Price = tour.Price,
                TourImage = tour.TourImage,

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
                IsActive = createTours.IsActive,
                Price = (decimal)createTours.Price,
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
                IsActive = tour.IsActive,
                Price = tour.Price,
                TourImage = fileName

            };




        }

        public async Task<ToursDTO> UpdateToursAsync(int id, CreateTours createTours)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            if (createTours.TourImage != null)
            {
                tour.TourImage = await SaveImageFileAsync(createTours.TourImage);
            }

            tour.TourName = createTours.TourName ?? tour.TourName;
            tour.Description = createTours.Description ?? tour.Description;
            tour.Price = createTours.Price ?? tour.Price;
            tour.Duration = createTours.Duration ?? tour.Duration;


            if (createTours.TourImage != null)
            {
                if (!string.IsNullOrEmpty(tour.TourImage))
                {
                    await DeleteImageFileAsync(tour.TourImage);
                }
                var fileName = await SaveImageFileAsync(createTours.TourImage);
                tour.TourImage = fileName;
            }

            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();

            return new ToursDTO
            {
                TourId = tour.TourId,
                TourName = tour.TourName,
                Description = tour.Description,
                Duration = tour.Duration,
                Price = tour.Price,
                IsActive = tour.IsActive,
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
