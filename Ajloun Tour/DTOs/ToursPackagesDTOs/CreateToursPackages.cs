using Ajloun_Tour.DTOs2.PackagesDTOS;

namespace Ajloun_Tour.DTOs.ToursPackagesDTOs
{
    public class CreateToursPackages
    {
        public int TourId { get; set; }
        public int PackageId { get; set; }
        public bool? IsActive { get; set; }
        public IFormFile? Image { get; set; }

    }
}
