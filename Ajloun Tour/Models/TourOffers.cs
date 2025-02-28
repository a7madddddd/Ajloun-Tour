using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class TourOffers
    {
        public int TourId { get; set; }
        public int OfferId { get; set; }

        public virtual Tours Tour { get; set; } = null!;
    }
}
