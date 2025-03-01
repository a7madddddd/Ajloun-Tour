namespace Ajloun_Tour.DTOs2.PackagesDTOS
{
    public class PackagesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Details { get; set; }
        public decimal? Price { get; set; }
        public int? TourDays { get; set; }
        public int? TourNights { get; set; }
        public int? NumberOfPeople { get; set; }
    }
}
