using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class JobImage
    {
        public int ImageId { get; set; }
        public int? JobId { get; set; }
        public string? ImageUrl { get; set; }

        public virtual Job? Job { get; set; }
    }
}
