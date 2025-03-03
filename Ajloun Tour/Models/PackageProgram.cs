using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class PackageProgram
    {
        public int PackageProgramId { get; set; }
        public int PackageId { get; set; }
        public int ProgramId { get; set; }
        public int DayNumber { get; set; }
        public DateTime? ProgramDate { get; set; }
        public string? CustomTitle { get; set; }
        public string? CustomDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Package Package { get; set; } = null!;
        public virtual Program Program { get; set; } = null!;
    }
}
