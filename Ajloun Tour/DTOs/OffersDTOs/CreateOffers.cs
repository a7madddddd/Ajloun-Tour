namespace Ajloun_Tour.DTOs.OffersDTOs
{
    public class CreateOffers
    {
        public string Title { get; set; } = null!;
        public decimal? DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
