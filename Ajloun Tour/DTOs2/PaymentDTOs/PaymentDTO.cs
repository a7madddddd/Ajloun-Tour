namespace Ajloun_Tour.DTOs2.PaymentDTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int? BookingId { get; set; }
        public int? UserId { get; set; }
        public int? GatewayId { get; set; }
        public int? CartId { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
