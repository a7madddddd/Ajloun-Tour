namespace Ajloun_Tour.DTOs.ToursOffersDTOs
{
    public class ToursOffersDTO
    {
        public int TourId { get; set; }
        public int OfferId { get; set; }
        public string? TourName { get; set; }
        public string? OfferTitle { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsActive { get; set; }
        public string? Image { get; set; }
        public int? Peapole { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Map { get; set; }


    }

}

