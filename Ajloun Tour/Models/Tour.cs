using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Tour
    {
        public Tour()
        {
            Bookings = new HashSet<Booking>();
            Reviews = new HashSet<Review>();
            Offers = new HashSet<Offer>();
            Packages = new HashSet<Package>();
            TourOffers = new HashSet<TourOffer>();
            TourPackages = new HashSet<TourPackage>();
        }

        public int TourId { get; set; }
        public string TourName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Duration { get; set; }
        public bool? IsActive { get; set; }
        public string? TourImage { get; set; }
        public string? Details { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
        public virtual ICollection<Package> Packages { get; set; }

        public virtual ICollection<TourOffer> TourOffers { get; set; }
        public virtual ICollection<TourPackage> TourPackages { get; set; }
    }
}
