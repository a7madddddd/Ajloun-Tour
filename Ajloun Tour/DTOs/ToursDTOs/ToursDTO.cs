namespace Ajloun_Tour.DTOs.ToursDTOs
{
    public class ToursDTO
    {
        public int TourId { get; set; }
        public string TourName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Details { get; set; }
        public string? Duration { get; set; }
        public bool? IsActive { get; set; }
        public string? TourImage { get; set; }
        public string? Location { get; set; }
        public string? Map { get; set; }
        public int? Limit { get; set; }

    }
}
