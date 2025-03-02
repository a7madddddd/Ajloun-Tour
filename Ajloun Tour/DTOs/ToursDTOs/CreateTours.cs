using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.DTOs.ToursDTOs
{
    public class CreateTours
    {
        [FromForm]
        public string? TourName { get; set; } = null!;
        [FromForm]
        public string? Description { get; set; }
        [FromForm]
        public decimal? Price { get; set; }
        [FromForm]
        public string? Duration { get; set; }
        [FromForm]
        public string? Details { get; set; }
        [FromForm]
        public bool? IsActive { get; set; }
        [FromForm]
        public IFormFile? TourImage { get; set; }
        [FromForm]
        public string? Location { get; set; }
        [FromForm]
        public string? Map { get; set; }
        [FromForm]
        public int? Limit { get; set; }


    }
}
