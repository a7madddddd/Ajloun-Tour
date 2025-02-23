using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class BookingOption
    {
        public BookingOption()
        {
            BookingOptionSelections = new HashSet<BookingOptionSelection>();
        }

        public int OptionId { get; set; }
        public string OptionName { get; set; } = null!;

        public virtual ICollection<BookingOptionSelection> BookingOptionSelections { get; set; }
    }
}
