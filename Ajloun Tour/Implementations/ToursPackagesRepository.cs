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
                 .Include(to => to.Tour)
                 .Include(to => to.Package)
                 .Select(to => new ToursPackagesDTO
                 {
                     TourId = to.TourId,
                     PackageId = to.PackageId,
                     // تعبئة البيانات الخاصة بـ PackagesDTO من البيانات الموجودة في Package
                     packagesDTO = new PackagesDTO
                     {
                         // ضع هنا الحقول التي تحتاجها من Package
                         Id = to.Package.Id,
                         Name = to.Package.Name,
                         Details = to.Package.Details,
                         Price = to.Package.Price
                     }
                 })
                 .AsNoTracking() // لمنع تتبع البيانات وتحميل العلاقات غير الضرورية
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
                    packagesDTO = new PackagesDTO
                    {
                        // ضع هنا الحقول التي تحتاجها من Package
                        Id = to.Package.Id,
                        Name = to.Package.Name,
                        Details = to.Package.Details,
                        Price = to.Package.Price
                    }

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

        public async Task<bool> AddTourPackage(CreateToursPackages createToursPackages)
        {
            try
            {
                Console.WriteLine($" Received: TourId={createToursPackages.TourId}, PackageId={createToursPackages.PackageId}");

                var tourExists = await _context.Tours.AnyAsync(t => t.TourId == createToursPackages.TourId);
                var packageExists = await _context.Packages.AnyAsync(p => p.Id == createToursPackages.PackageId);

                if (!tourExists || !packageExists)
                {
                    Console.WriteLine(" Error: Tour or Package does not exist.");
                    return false;
                }

                var tourPackage = new TourPackage
                {
                    TourId = createToursPackages.TourId,
                    PackageId = createToursPackages.PackageId,

                };

                await _context.TourPackages.AddAsync(tourPackage);
                Console.WriteLine($" Added TourPackage: TourId={tourPackage.TourId}, PackageId={tourPackage.PackageId}");

                var changes = await _context.SaveChangesAsync();
                Console.WriteLine($" Database changes: {changes}");

                return changes > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❗ Error in AddTourPackage: {ex.Message}");
                return false;
            }
        }

        public async Task<ToursPackagesDTO> UpdateTourPackages(int TourId, CreateToursPackages createToursPackages)
        {
            var existingTourPackages = await _context.TourPackages
                            .FirstOrDefaultAsync(to => to.TourId == TourId);

            if (existingTourPackages == null)
            {
                throw new KeyNotFoundException($"No tour Packages found with tour ID: {TourId}");
            }

            var packagesExists = await _context.Packages
                .AnyAsync(o => o.Id == createToursPackages.PackageId);

            if (!packagesExists)
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

            existingTourPackages.PackageId = createToursPackages.PackageId;

            if (createToursPackages.TourId != TourId)
            {
                var newPackageExists = await _context.Tours
                    .AnyAsync(t => t.TourId == createToursPackages.TourId);

                if (!newPackageExists)
                {
                    throw new InvalidOperationException($"Package with ID {createToursPackages.TourId} does not exist");
                }

                existingTourPackages.TourId = createToursPackages.TourId;
            }

            await _context.SaveChangesAsync();

            return new ToursPackagesDTO
            {
                TourId = existingTourPackages.TourId,
                PackageId = existingTourPackages.PackageId,
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
