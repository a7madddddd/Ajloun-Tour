using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Testomonial
    {
        public int TestomoId { get; set; }
        public string? Message { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
