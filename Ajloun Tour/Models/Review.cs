using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public int? TourId { get; set; }
        public int? UserId { get; set; }
        public int? Rating { get; set; }
        public string? Subject { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? PackageId { get; set; }
        public int? OfferId { get; set; }
        public bool? IsActive { get; set; } = false;

        public virtual Offer? Offer { get; set; }
        public virtual Package? Package { get; set; }
        public virtual Tour? Tour { get; set; }
        public virtual User? User { get; set; }
    }
}
