namespace Ajloun_Tour.DTOs.ToursDTOs
{
    public class CreateTours
    {
        public string TourName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Duration { get; set; }
        public bool? IsActive { get; set; }
        public FormFile? TourImage { get; set; }
    }
}
