namespace Ajloun_Tour.DTOs.OffersDTOs
{
    public class CreateOffers
    {
        public string Title { get; set; } = null!;
        public decimal? DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IFormFile? Image { get; set; }
        public int? Peapole { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; } = false;
        public string? Description { get; set; }


    }
}
