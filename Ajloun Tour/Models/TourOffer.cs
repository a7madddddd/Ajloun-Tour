using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ajloun_Tour.Models
{
    [Table("TourOffers")]
    public class TourOffer
    {
        [Key]
        [Column(Order = 0)]
        public int TourId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int OfferId { get; set; }

        public Tour? Tour { get; set; }
        public Offer? Offer { get; set; }




    }
}
