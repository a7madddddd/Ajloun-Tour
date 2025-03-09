using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs2.PaymentDTOs
{
    public class CreatePayment
    {
        [Required]
        public int? BookingID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public int GatewayID { get; set; }
        public int? CartId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TransactionId { get; set; } 

    }
}
