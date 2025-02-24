using System.ComponentModel.DataAnnotations.Schema;

namespace Ajloun_Tour.Models
{
    [Table("TourOffers")]
    public class TourOffer
    {
        public int TourId { get; set; }
        public int OfferId { get; set; }

        public Tour? Tour { get; set; }
        public Offer? Offer { get; set; }


        public virtual ICollection<TourOffer> TourOffers { get; set; } = new List<TourOffer>(); // ✅ Add this

    }
}
