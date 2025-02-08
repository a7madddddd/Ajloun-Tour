using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public int? TourId { get; set; }
        public int? UserId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Tour? Tour { get; set; }
        public virtual User? User { get; set; }
    }
}
