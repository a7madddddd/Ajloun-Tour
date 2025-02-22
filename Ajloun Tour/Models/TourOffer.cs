namespace Ajloun_Tour.Models
{
    public class TourOffer
    {
        public int TourId { get; set; }
        public int OfferId { get; set; }

        public Tour? Tour { get; set; }
        public Offer? Offer { get; set; }
    }
}
