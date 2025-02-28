using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class TourPackages
    {
        public int TourId { get; set; }
        public int PackageId { get; set; }

        public virtual Tours Tour { get; set; } = null!;
    }
}
