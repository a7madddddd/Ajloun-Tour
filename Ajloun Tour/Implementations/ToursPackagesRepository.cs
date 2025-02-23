using Ajloun_Tour.DTOs.ToursOffersDTOs;
using Ajloun_Tour.DTOs.ToursPackages;
using Ajloun_Tour.DTOs.ToursPackagesDTOs;
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
               Name = to.Tour.TourName,
               Details = to.Package.Details,
               Price = (decimal)to.Package.Price

           })
           .ToListAsync();
        }
        public async Task<ToursPackagesDTO> GetTourPackageById(int TourId, int PackageId)
        {
            return await _context.TourPackages
                      .Include(to => to.Tour)
                      .Include(to => to.Package)
                      .Where(to => to.TourId == TourId && to.PackageId == PackageId)
                      .Select(to => new ToursPackagesDTO
                      {
                          TourId = to.TourId,
                          PackageId = to.PackageId,
                          Name = to.Tour.TourName,
                          Details = to.Package.Details,
                          Price = (decimal)to.Package.Price
                      })
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

        public async Task<bool> AddTourPackage(TourPackage tourPackage)
        {
            try
            {
                await _context.TourPackages.AddAsync(tourPackage);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
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
