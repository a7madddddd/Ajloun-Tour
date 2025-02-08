using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
        }

        public int UserId { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UserImage { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
