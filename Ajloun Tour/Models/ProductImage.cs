using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class ProductImage
    {
        public int ImageId { get; set; }
        public int? ProductId { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsThumbnail { get; set; }

        public virtual Product? Product { get; set; }
    }
}
