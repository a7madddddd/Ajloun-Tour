using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Carts = new HashSet<Cart>();
            Payments = new HashSet<Payment>();
            Reviews = new HashSet<Review>();
            Testomonials = new HashSet<Testomonial>();
        }

        public int UserId { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UserImage { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Testomonial> Testomonials { get; set; }
    }
}
