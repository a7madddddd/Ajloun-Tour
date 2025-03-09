using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class PaymentDetail
    {
        public int PaymentDetailId { get; set; }
        public int? PaymentId { get; set; }
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpiryDate { get; set; }
        public string? Cvv { get; set; }
        public string? BillingAddress { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingCountry { get; set; }
        public string? BillingZipCode { get; set; }
        public string? AdditionalNotes { get; set; }

        public virtual Payment? Payment { get; set; }
    }
}
