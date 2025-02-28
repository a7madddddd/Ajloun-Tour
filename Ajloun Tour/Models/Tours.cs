using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Tours
    {
        public Tours()
        {
            TourOffers = new HashSet<TourOffers>();
            TourPackages = new HashSet<TourPackages>();
        }

        public int TourId { get; set; }
        public string TourName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Duration { get; set; }
        public bool? IsActive { get; set; }
        public string? TourImage { get; set; }
        public string? Details { get; set; }
        public string? Location { get; set; }

        public virtual ICollection<TourOffers> TourOffers { get; set; }
        public virtual ICollection<TourPackages> TourPackages { get; set; }
    }
}
