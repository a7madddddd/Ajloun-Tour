using Ajloun_Tour.DTOs.ToursOffersDTOs;
using Ajloun_Tour.DTOs.ToursPackages;
using Ajloun_Tour.DTOs.ToursPackagesDTOs;
using Ajloun_Tour.Models;
using ToursPackagesDTO = Ajloun_Tour.DTOs.ToursPackages.ToursPackagesDTO;

namespace Ajloun_Tour.Reposetories
{
    public interface IToursPackagesRepository
    {
        Task<IEnumerable<ToursPackagesDTO>> GetAllToursPackages();

        Task<ToursPackagesDTO> GetTourPackageById(int TourId, int PackageId);

        Task<bool> AddTourPackage(CreateToursPackages createToursPackages);

        Task<ToursPackagesDTO> UpdateTourPackages(int TourId, CreateToursPackages createToursPackages);

        Task<bool> DeleteTourPackages(int TourId, int PackageId);

        //Task<IEnumerable<ToursPackagesDTO>> GetActivePackages();
    }
}
