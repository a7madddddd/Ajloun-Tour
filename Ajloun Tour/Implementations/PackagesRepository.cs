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

            };
        }
        public async Task<PackagesDTO> AddPackagesAsync(CreatePackages createPackages)
        {
            var Package = new Package
            {

                Name = createPackages.Name,
                Details = createPackages.Details,
                Price = createPackages.Price,

            };

            await _context.Packages.AddAsync(Package);
            _context.SaveChanges();

            return new PackagesDTO
            {
                Id = Package.Id,
                Name = createPackages.Name,
                Details = createPackages.Details,
                Price = createPackages.Price,

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
           


            _context.Packages.Update(updatePackage);
            await _context.SaveChangesAsync();


            return new PackagesDTO
            {

                Id = updatePackage.Id,
                Name = updatePackage.Name,
                Price = updatePackage.Price,
                Details = updatePackage.Details,
              
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
