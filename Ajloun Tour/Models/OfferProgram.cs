using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class OfferProgram
    {
        public int OfferProgramId { get; set; }
        public int OfferId { get; set; }
        public int ProgramId { get; set; }
        public int DayNumber { get; set; }
        public DateTime? ProgramDate { get; set; }
        public string? CustomTitle { get; set; }
        public string? CustomDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Offer Offer { get; set; } = null!;
        public virtual Program Program { get; set; } = null!;
    }
}
