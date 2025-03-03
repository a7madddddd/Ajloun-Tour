using Ajloun_Tour.DTOs.ToursDTOs;
using Ajloun_Tour.DTOs.ToursOffersDTOs;
using Ajloun_Tour.DTOs.ToursPackages;
using Ajloun_Tour.DTOs.ToursPackagesDTOs;
using Ajloun_Tour.DTOs2.PackagesDTOS;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class ToursPackagesRepository : IToursPackagesRepository
    {
        private readonly MyDbContext _context;
        private readonly string _imageDirectory;

        public ToursPackagesRepository(MyDbContext context)
        {
            _context = context;
            _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PakagesImages");
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




        public async Task<IEnumerable<ToursPackagesDTO>> GetAllToursPackages()
        {
            return await _context.TourPackages
                .Include(tp => tp.Package)
                .Select(tp => new ToursPackagesDTO
                {
                    TourId = tp.TourId,
                    PackageId = tp.PackageId,
                    IsActive = tp.IsActive,
                    PackageName = tp.Package.Name,
                    Details = tp.Package.Details,
                    Price = tp.Package.Price,
                    TourDays = tp.Package.TourDays,
                    TourNights = tp.Package.TourNights,
                    NumberOfPeople = tp.Package.NumberOfPeople,
                    Map = tp.Package.Map,
                    Location = tp.Package.Location,
                    Image = tp.Image,
                })
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<ToursPackagesDTO> GetTourPackageById(int TourId, int PackageId)
        {
            return await _context.TourPackages
                .Include(to => to.Package)  // Include the related Package entity
                .Where(to => to.TourId == TourId && to.PackageId == PackageId)
                .Select(to => new ToursPackagesDTO
                {
                    TourId = to.TourId,
                    PackageId = to.PackageId,
                    IsActive = to.IsActive,
                    PackageName = to.Package.Name, 
                    Details = to.Package.Details,   
                    Price = to.Package.Price,      
                    TourDays = to.Package.TourDays, 
                    TourNights = to.Package.TourNights,  
                    NumberOfPeople = to.Package.NumberOfPeople, 
                    Map = to.Package.Map,
                    Location = to.Package.Location,
                    Image = to.Image
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }




        //public async Task<IEnumerable<TourPackage>> GetActivePackages()
        //{
        //    var currentDate = DateTime.UtcNow.Date;
        //    return await _context.TourPackages
        //        .Include(to => to.Tour)
        //        .Include(to => to.Package)
        //        .Where(to => to.Package. <= currentDate && to.Offer.EndDate >= currentDate)
        //        .Select(to => new ToursOffersDTO
        //        {
        //            TourId = to.TourId,
        //            OfferId = to.OfferId,
        //            TourName = to.Tour.TourName,
        //            OfferTitle = to.Offer.Title,
        //            DiscountPercentage = (decimal)to.Offer.DiscountPercentage,
        //            StartDate = (DateTime)to.Offer.StartDate,
        //            EndDate = (DateTime)to.Offer.EndDate
        //        })
        //        .ToListAsync();
        //}

        public async Task<ToursPackagesDTO> AddTourPackage(CreateToursPackages createToursPackages)
        {
            try
            {
                if (createToursPackages.Image == null)
                {
                    throw new ArgumentNullException(nameof(createToursPackages.Image), "Image file is required.");
                }

                // Save the image file and get the file name
                var fileName = await SaveImageFileAsync(createToursPackages.Image);

                Console.WriteLine($"Received: TourId={createToursPackages.TourId}, PackageId={createToursPackages.PackageId}");

                var tourExists = await _context.Tours.AnyAsync(t => t.TourId == createToursPackages.TourId);
                var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == createToursPackages.PackageId);

                if (!tourExists)
                {
                    Console.WriteLine("❌ Error: Tour does not exist.");
                    return null;
                }

                var tourPackage = new TourPackage
                {
                    TourId = createToursPackages.TourId,
                    PackageId = package.Id,
                    IsActive = createToursPackages.IsActive,
                    Image = fileName, 
                };

                await _context.TourPackages.AddAsync(tourPackage);
                Console.WriteLine($"✅ Added TourPackage: TourId={tourPackage.TourId}, PackageId={tourPackage.PackageId}");

                var changes = await _context.SaveChangesAsync();
                Console.WriteLine($"🔄 Database changes: {changes}");

                if (changes > 0)
                {
                    return new ToursPackagesDTO
                    {
                        TourId = tourPackage.TourId,
                        PackageId = tourPackage.PackageId,
                        IsActive = tourPackage.IsActive,
                        Image = fileName,  
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❗ Error in AddTourPackage: {ex.Message}");
                return null;
            }
        }



        public async Task<ToursPackagesDTO> UpdateTourPackages(int id, CreateToursPackages createToursPackages)
        {
            // Then use tourId parameter to find the right record
            var existingTourPackage = await _context.TourPackages
                .FirstOrDefaultAsync(tp => tp.TourId == id);

            if (existingTourPackage == null)
            {
                throw new KeyNotFoundException($"No tour package found with ID: {id}");
            }

            // If PackageId is updated, check if the new Package exists
            if (createToursPackages.PackageId != existingTourPackage.PackageId)
            {
                var packageExists = await _context.Packages
                    .AnyAsync(p => p.Id == createToursPackages.PackageId);

                if (!packageExists)
                {
                    throw new InvalidOperationException($"Package with ID {createToursPackages.PackageId} does not exist.");
                }

                existingTourPackage.PackageId = createToursPackages.PackageId;  // Update PackageId
            }

            // If TourId is updated, check if the new Tour exists
            if (createToursPackages.TourId != existingTourPackage.TourId)
            {
                var tourExists = await _context.Tours
                    .AnyAsync(t => t.TourId == createToursPackages.TourId);

                if (!tourExists)
                {
                    throw new InvalidOperationException($"Tour with ID {createToursPackages.TourId} does not exist.");
                }

                existingTourPackage.TourId = createToursPackages.TourId;  // Update TourId
            }

            // Update IsActive if provided
            if (createToursPackages.IsActive.HasValue)
            {
                existingTourPackage.IsActive = createToursPackages.IsActive.Value;
            }

            // Handle Image update if a new image is provided
            if (createToursPackages.Image != null)
            {
                // Save the new image and update the Image field
                var newImageFileName = await SaveImageFileAsync(createToursPackages.Image);
                existingTourPackage.Image = newImageFileName;
            }

            // Save changes to the database
            _context.TourPackages.Update(existingTourPackage);
            await _context.SaveChangesAsync();

            return new ToursPackagesDTO
            {
                TourId = existingTourPackage.TourId,
                PackageId = existingTourPackage.PackageId,
                IsActive = existingTourPackage.IsActive,
                Image = existingTourPackage.Image,
            };
        }

        public async Task<bool> DeleteTourPackages(int TourId, int PackageId)
        {
            var tourPackage = await _context.TourPackages
                                   .FirstOrDefaultAsync(to => to.TourId == TourId && to.PackageId == PackageId);

            if (tourPackage == null)
                return false;

            _context.TourPackages.Remove(tourPackage);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
