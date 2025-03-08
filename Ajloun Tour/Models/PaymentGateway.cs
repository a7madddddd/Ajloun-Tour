using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class PaymentGateway
    {
        public PaymentGateway()
        {
            Payments = new HashSet<Payment>();
        }

        public int GatewayId { get; set; }
        public string? GatewayName { get; set; }
        public bool IsActive { get; set; }
        public string? ApiKey { get; set; }
        public string? SecretKey { get; set; }
        public string? WebhookUrl { get; set; }
        public string? Environment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
