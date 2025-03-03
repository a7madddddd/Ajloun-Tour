using Ajloun_Tour.DTOs2.PackagesDTOS;

namespace Ajloun_Tour.DTOs.ToursPackages
{
    public class ToursPackagesDTO
    {
        public int TourId { get; set; }
        public int PackageId { get; set; }
        public bool? IsActive { get; set; }
        public string PackageName { get; set; } = null!;
        public string? Details { get; set; }
        public decimal? Price { get; set; }
        public int? TourDays { get; set; }
        public int? TourNights { get; set; }
        public int? NumberOfPeople { get; set; }
        public string? Image { get; set; }

    }
}
