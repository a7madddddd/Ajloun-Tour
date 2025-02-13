using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Project
    {
        public int ProjectId { get; set; }
        public string? Status { get; set; }
        public string? ProjectImage { get; set; }
        public int? AdminId { get; set; }

        public virtual Admin? Admin { get; set; }
    }
}
