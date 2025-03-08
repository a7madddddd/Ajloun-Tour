namespace Ajloun_Tour.DTOs2.PaymentHistoryDTOs
{
    public class PaymentHistoryDTO
    {
        public int PaymentHistoryId { get; set; }
        public int? PaymentId { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
