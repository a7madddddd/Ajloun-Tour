using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Program
    {
        public Program()
        {
            OfferPrograms = new HashSet<OfferProgram>();
            PackagePrograms = new HashSet<PackageProgram>();
            TourPrograms = new HashSet<TourProgram>();
        }

        public int ProgramId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? DefaultDayNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<OfferProgram> OfferPrograms { get; set; }
        public virtual ICollection<PackageProgram> PackagePrograms { get; set; }
        public virtual ICollection<TourProgram> TourPrograms { get; set; }
    }
}
