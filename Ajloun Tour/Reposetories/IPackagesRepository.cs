using Ajloun_Tour.DTOs2.PackagesDTOS;
using Ajloun_Tour.Models;

namespace Ajloun_Tour.Reposetories
{
    public interface IPackagesRepository
    {
        Task<IEnumerable<PackagesDTO>> GetALLPackages();

        Task<PackagesDTO> GetPackagesById(int id);

        Task<PackagesDTO> AddPackagesAsync(CreatePackages createPackages);

        Task<PackagesDTO> UpdatePackagesAsync(int id, CreatePackages createPackages);

        Task DeletePackagesById(int id);
    }
}
