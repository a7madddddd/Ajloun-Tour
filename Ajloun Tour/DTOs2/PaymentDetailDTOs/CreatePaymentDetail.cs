using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs2.PaymentDetailDTOs
{
    public class CreatePaymentDetail
    {
        [Required]
        public int PaymentID { get; set; }

        [Required]
        [StringLength(100)]
        public string CardHolderName { get; set; }

        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$")]
        public string ExpiryDate { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; }

        [Required]
        public string BillingAddress { get; set; }

        [Required]
        public string BillingCity { get; set; }

        [Required]
        public string BillingCountry { get; set; }

        [Required]
        public string BillingZipCode { get; set; }
        public string? AdditionalNotes { get; set; }

    }
}
