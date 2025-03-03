using Ajloun_Tour.DTOs.ContactDTOs;
using Ajloun_Tour.DTOs2.PackagesDTOS;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class PackagesRepository : IPackagesRepository
    {
        private readonly MyDbContext _context;

        public PackagesRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PackagesDTO>> GetALLPackages()
        {
            var Packages = await _context.Packages.ToListAsync();

            return Packages.Select(c => new PackagesDTO
            {

                Id = c.Id,
                Name = c.Name,
                Details = c.Details,
                Price = c.Price,
                NumberOfPeople = c.NumberOfPeople,
                TourDays = c.TourDays,
                TourNights = c.TourNights,
                Location = c.Location,
                Map = c.Map,
                IsActive = c.IsActive,

            });
        }
        public async Task<PackagesDTO> GetPackagesById(int id)
        {
            var Package = await _context.Packages.FindAsync(id);

            if (Package == null)
            {

                throw new Exception("This Package Is Not Defined");
            }

            return new PackagesDTO
            {

                Id = Package.Id,
                Name = Package.Name,
                Details = Package.Details,
                Price = Package.Price,
                NumberOfPeople = Package.NumberOfPeople,
                TourDays = Package.TourDays,
                TourNights = Package.TourNights,
                Location = Package.Location,
                Map = Package.Map,
                IsActive = Package.IsActive,

            };
        }
        public async Task<PackagesDTO> AddPackagesAsync(CreatePackages createPackages)
        {
            Console.WriteLine($"Name: {createPackages.Name}, TourDays: {createPackages.TourDays}, TourNights: {createPackages.TourNights}");

            var Package = new Package
            {
                Name = createPackages.Name,
                Details = createPackages.Details,
                Price = createPackages.Price,
                NumberOfPeople = createPackages.NumberOfPeople,
                TourNights = createPackages.TourNights,
                TourDays = createPackages.TourDays,
                Location = createPackages.Location,
                Map = createPackages.Map,
                IsActive=createPackages.IsActive,
            };

            await _context.Packages.AddAsync(Package);
            await _context.SaveChangesAsync(); // Ensure async save

            return new PackagesDTO
            {
                Id = Package.Id,
                Name = Package.Name,
                Details = Package.Details,
                Price = Package.Price,
                TourDays = Package.TourDays,
                NumberOfPeople = Package.NumberOfPeople,
                TourNights = Package.TourNights,
                Location= Package.Location,
                Map = Package.Map,
                IsActive = Package.IsActive,
            };
        }


        public async Task<PackagesDTO> UpdatePackagesAsync(int id, CreatePackages createPackages)
        {
            var updatePackage = await _context.Packages.FindAsync(id);

            if (updatePackage == null)
            {

                throw new ArgumentNullException(nameof(updatePackage));
            };

            updatePackage.Name = createPackages.Name ?? updatePackage.Name;
            updatePackage.Details = createPackages.Details ?? updatePackage.Details;
            updatePackage.Price = createPackages.Price ?? updatePackage.Price;
            updatePackage.NumberOfPeople = createPackages.NumberOfPeople ?? updatePackage.NumberOfPeople;
            updatePackage.TourDays = createPackages.TourDays ?? updatePackage.TourDays;
            updatePackage.TourNights = createPackages?.TourNights ?? updatePackage.TourNights;
            updatePackage.Location = createPackages?.Location ?? updatePackage.Location;
            updatePackage.Map = createPackages?.Map ?? updatePackage.Map;
            updatePackage.IsActive = createPackages?.IsActive ?? updatePackage.IsActive;



            _context.Packages.Update(updatePackage);
            await _context.SaveChangesAsync();


            return new PackagesDTO
            {

                Id = updatePackage.Id,
                Name = updatePackage.Name,
                Price = updatePackage.Price,
                Details = updatePackage.Details,
                TourNights = updatePackage.TourNights,
                TourDays = updatePackage.TourDays,
                NumberOfPeople = updatePackage.NumberOfPeople,
                Location = updatePackage.Location,
                Map = updatePackage.Map,
                IsActive = updatePackage.IsActive,

            };
        }
        public async Task DeletePackagesById(int id)
        {
            var deletedPackage = await _context.Packages.FindAsync(id);

            if (deletedPackage == null)
            {

                throw new ArgumentNullException(nameof(deletedPackage));

            }

            _context.Packages.Remove(deletedPackage);
            await _context.SaveChangesAsync();
        }



    }
}
