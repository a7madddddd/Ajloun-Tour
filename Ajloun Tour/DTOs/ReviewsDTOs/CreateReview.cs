namespace Ajloun_Tour.DTOs.ReviewsDTOs
{
    public class CreateReview
    {
        public int? TourId { get; set; }
        public int? UserId { get; set; }
        public int? Rating { get; set; }
        public string? Subject { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? PackageId { get; set; }
        public int? OfferId { get; set; }
        public bool? IsActive { get; set; }


    }
}
