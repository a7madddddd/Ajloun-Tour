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

        public ToursPackagesRepository(MyDbContext context)
        {
            _context = context;
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
                    NumberOfPeople = tp.Package.NumberOfPeople
                })
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<ToursPackagesDTO> GetTourPackageById(int TourId, int PackageId)
        {
            return await _context.TourPackages
                .Where(to => to.TourId == TourId && to.PackageId == PackageId)
                .Select(to => new ToursPackagesDTO
                {
                    TourId = to.TourId,
                    PackageId = to.PackageId,
                    IsActive = to.IsActive,


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
                    IsActive = createToursPackages.IsActive = false,
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
                        IsActive= tourPackage.IsActive,

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



        public async Task<ToursPackagesDTO> UpdateTourPackages(int TourId, CreateToursPackages createToursPackages)
        {
            var existingTourPackages = await _context.TourPackages
                            .FirstOrDefaultAsync(to => to.TourId == TourId);

            if (existingTourPackages == null)
            {
                throw new KeyNotFoundException($"No tour package found with Tour ID: {TourId}");
            }

            var packageExists = await _context.Packages
                .AnyAsync(o => o.Id == createToursPackages.PackageId);

            if (!packageExists)
            {
                throw new InvalidOperationException($"Package with ID {createToursPackages.PackageId} does not exist");
            }

            var duplicateExists = await _context.TourPackages
                .AnyAsync(to =>
                    to.TourId == createToursPackages.TourId &&
                    to.PackageId == createToursPackages.PackageId &&
                    to.TourId != TourId);

            if (duplicateExists)
            {
                throw new InvalidOperationException("This Tour-Package relationship already exists");
            }

            // Update package and tour IDs
            existingTourPackages.PackageId = createToursPackages.PackageId;

            if (createToursPackages.TourId != TourId)
            {
                var newTourExists = await _context.Tours
                    .AnyAsync(t => t.TourId == createToursPackages.TourId);

                if (!newTourExists)
                {
                    throw new InvalidOperationException($"Tour with ID {createToursPackages.TourId} does not exist");
                }

                existingTourPackages.TourId = createToursPackages.TourId;
            }

            // Update IsActive status
            if (createToursPackages.IsActive.HasValue) // Ensure IsActive is provided in the request
            {
                existingTourPackages.IsActive = createToursPackages.IsActive.Value;
            }

            await _context.SaveChangesAsync();

            return new ToursPackagesDTO
            {
                TourId = existingTourPackages.TourId,
                PackageId = existingTourPackages.PackageId,
                IsActive = existingTourPackages.IsActive
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
