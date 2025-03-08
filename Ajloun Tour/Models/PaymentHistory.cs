using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class PaymentHistory
    {
        public int PaymentHistoryId { get; set; }
        public int? PaymentId { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Payment? Payment { get; set; }
    }
}
