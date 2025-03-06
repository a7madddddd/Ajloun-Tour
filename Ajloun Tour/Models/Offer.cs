using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Offer
    {
        public Offer()
        {
            Bookings = new HashSet<Booking>();
            OfferPrograms = new HashSet<OfferProgram>();
            Reviews = new HashSet<Review>();
            Tours = new HashSet<Tour>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal? DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public string? Image { get; set; }
        public int? Peapole { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<OfferProgram> OfferPrograms { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }
    }
}
