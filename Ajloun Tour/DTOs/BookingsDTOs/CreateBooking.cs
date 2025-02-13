namespace Ajloun_Tour.DTOs.BookingsDTOs
{
    public class CreateBooking
    {
        public int? TourId { get; set; }
        public int? UserId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
