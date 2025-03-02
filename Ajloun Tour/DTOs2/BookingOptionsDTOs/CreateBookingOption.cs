using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs2.BookingOptionsDTOs
{
    public class CreateBookingOption
    {
        [Required]
        [StringLength(100)]
        public string? OptionName { get; set; }
        public decimal? OptionPrice { get; set; }

    }
}
