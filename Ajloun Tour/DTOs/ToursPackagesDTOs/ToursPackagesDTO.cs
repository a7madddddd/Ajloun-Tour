using Ajloun_Tour.DTOs2.PackagesDTOS;

namespace Ajloun_Tour.DTOs.ToursPackages
{
    public class ToursPackagesDTO
    {
        public int TourId { get; set; }
        public int PackageId { get; set; }

        public PackagesDTO packagesDTO { get; set; }
                                                                                                                                                
    }
}
