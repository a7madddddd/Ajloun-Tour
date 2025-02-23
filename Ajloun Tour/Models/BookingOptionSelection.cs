using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class BookingOptionSelection
    {
        public int SelectionId { get; set; }
        public int BookingId { get; set; }
        public int OptionId { get; set; }

        public virtual Booking Booking { get; set; } = null!;
        public virtual BookingOption Option { get; set; } = null!;
    }
}
