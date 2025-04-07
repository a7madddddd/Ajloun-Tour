using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs2.CartItemsDTOs
{
    public class CreateCartItemDTO
    {
        [Required]
        public int CartId { get; set; }
        public int? TourId { get; set; }
        public int? PackageId { get; set; }
        public int? OfferId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime SelectedDate { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public decimal? Option1Price { get; set; }
        public decimal? Option2Price { get; set; }
        public decimal? Option3Price { get; set; }
        public decimal? Option4Price { get; set; }

        public int? BookingId { get; set; }
        public int? ProductId { get; set; }

    }
}
