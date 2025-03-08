using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs2.PaymentHistoryDTOs
{
    public class CreatePaymentHistory
    {
        [Required]
        public int PaymentID { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
