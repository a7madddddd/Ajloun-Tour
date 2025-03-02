namespace Ajloun_Tour.DTOs.ToursDTOs
{
    public class TourWithRatingDTO
    {
        public int TourId { get; set; }
        public string TourName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Duration { get; set; }
        public bool IsActive { get; set; }
        public string TourImage { get; set; }
        public double? AverageRating { get; set; }
        public int? Limit { get; set; }

    }
}
