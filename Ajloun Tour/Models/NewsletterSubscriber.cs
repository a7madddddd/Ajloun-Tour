using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class NewsletterSubscriber
    {
        public int SubscriberId { get; set; }
        public string Email { get; set; } = null!;
        public DateTime? SubscribedAt { get; set; }
    }
}
