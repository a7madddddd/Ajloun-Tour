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
                    NumberOfPeople = tp.Package.NumberOfPeople,
                    Map = tp.Package.Map,
                    Location = tp.Package.Location,
                    Image = tp.Package.Image,
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
                    Image = to.Package.Image
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


                var tourExists = await _context.Tours.AnyAsync(t => t.TourId == createToursPackages.TourId);
                var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == createToursPackages.PackageId);

                if (!tourExists)
                {
                    return null;
                }

                var tourPackage = new TourPackage
                {
                    TourId = createToursPackages.TourId,
                    PackageId = package.Id,
                    IsActive = createToursPackages.IsActive,
                };

                await _context.TourPackages.AddAsync(tourPackage);

                var changes = await _context.SaveChangesAsync();

                if (changes > 0)
                {
                    return new ToursPackagesDTO
                    {
                        TourId = tourPackage.TourId,
                        PackageId = tourPackage.PackageId,
                        IsActive = tourPackage.IsActive,
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
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


            // Save changes to the database
            _context.TourPackages.Update(existingTourPackage);
            await _context.SaveChangesAsync();

            return new ToursPackagesDTO
            {
                TourId = existingTourPackage.TourId,
                PackageId = existingTourPackage.PackageId,
                IsActive = existingTourPackage.IsActive,
                Image = existingTourPackage.Package.Image,
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
