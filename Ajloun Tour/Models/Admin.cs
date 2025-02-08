using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? AdminImage { get; set; }
    }
}
