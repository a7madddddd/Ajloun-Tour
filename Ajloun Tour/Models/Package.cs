using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Package
    {
        public Package()
        {
            Tours = new HashSet<Tour>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Details { get; set; }
        public decimal? Price { get; set; }
        public int? TourDays { get; set; }
        public int? TourNights { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }
    }
}
