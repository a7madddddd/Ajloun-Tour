using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Payment
    {
        public Payment()
        {
            PaymentDetails = new HashSet<PaymentDetail>();
            PaymentHistories = new HashSet<PaymentHistory>();
        }

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

        public virtual Booking? Booking { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual PaymentGateway? Gateway { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
    }
}
