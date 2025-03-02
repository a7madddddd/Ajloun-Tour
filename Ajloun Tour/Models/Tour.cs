using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Tour
    {
        public Tour()
        {
            Bookings = new HashSet<Booking>();
            CartItems = new HashSet<CartItem>();
            Reviews = new HashSet<Review>();
            TourPackages = new HashSet<TourPackage>();
            TourPrograms = new HashSet<TourProgram>();
            Offers = new HashSet<Offer>();
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
        public string? Map { get; set; }
        public int? Limit { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<TourPackage> TourPackages { get; set; }
        public virtual ICollection<TourProgram> TourPrograms { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}
