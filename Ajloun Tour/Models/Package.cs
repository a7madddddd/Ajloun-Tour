using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Package
    {
        public Package()
        {
            Bookings = new HashSet<Booking>();
            CartItems = new HashSet<CartItem>();
            PackagePrograms = new HashSet<PackageProgram>();
            Reviews = new HashSet<Review>();
            TourPackages = new HashSet<TourPackage>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Details { get; set; }
        public decimal? Price { get; set; }
        public int? TourDays { get; set; }
        public int? TourNights { get; set; }
        public int? NumberOfPeople { get; set; }
        public bool? IsActive { get; set; }
        public string? Location { get; set; }
        public string? Map { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<PackageProgram> PackagePrograms { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<TourPackage> TourPackages { get; set; }
    }
}
