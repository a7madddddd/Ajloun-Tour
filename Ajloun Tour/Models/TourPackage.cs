using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class TourPackage
    {
        public int TourId { get; set; }
        public int PackageId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Package Package { get; set; } = null!;
        public virtual Tour Tour { get; set; } = null!;
    }
}
