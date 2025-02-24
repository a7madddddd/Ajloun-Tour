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

        public virtual ICollection<Tour> Tours { get; set; }
        public virtual ICollection<TourPackage> TourPackages { get; set; } = new List<TourPackage>(); // ✅ Add this

    }
}
