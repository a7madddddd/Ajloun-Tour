using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Tour
    {
        public Tour()
        {
            Bookings = new HashSet<Booking>();
        }

        public int TourId { get; set; }
        public string TourName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Duration { get; set; }
        public bool? IsActive { get; set; }
        public string? TourImage { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
