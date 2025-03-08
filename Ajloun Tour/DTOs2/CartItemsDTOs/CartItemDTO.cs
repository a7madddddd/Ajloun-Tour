using Ajloun_Tour.DTOs.BookingsDTOs;

namespace Ajloun_Tour.DTOs2.CartItemsDTOs
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int? TourId { get; set; }
        public int? PackageId { get; set; }
        public int? OfferId { get; set; }
        public int? Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime SelectedDate { get; set; }
        public int NumberOfPeople { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public decimal? Option1Price { get; set; }
        public decimal? Option2Price { get; set; }
        public decimal? Option3Price { get; set; }
        public decimal? Option4Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? BookingId { get; set; }
        public BookingDTO BookingDetails { get; set; }
        public bool IsFromBooking { get; set; }

    }
}
