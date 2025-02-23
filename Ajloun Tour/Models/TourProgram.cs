using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class TourProgram
    {
        public int ProgramId { get; set; }
        public int TourId { get; set; }
        public int DayNumber { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? ProgramDate { get; set; }

        public virtual Tour Tour { get; set; } = null!;
    }
}
