using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Booking
    {
        public Booking()
        {
            BookingOptionSelections = new HashSet<BookingOptionSelection>();
            CartItems = new HashSet<CartItem>();
            Payments = new HashSet<Payment>();
        }

        public int BookingId { get; set; }
        public int? TourId { get; set; }
        public int? UserId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? PackageId { get; set; }
        public int? OfferId { get; set; }

        public virtual Offer? Offer { get; set; }
        public virtual Package? Package { get; set; }
        public virtual Tour? Tour { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<BookingOptionSelection> BookingOptionSelections { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
