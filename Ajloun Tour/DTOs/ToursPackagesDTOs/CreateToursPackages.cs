namespace Ajloun_Tour.DTOs.ToursPackagesDTOs
{
    public class CreateToursPackages
    {
        public int TourId { get; set; }
        public int PackageId { get; set; }
        public string Name { get; set; } = null!;
        public string? Details { get; set; }
        public decimal? Price { get; set; }
    }
}
