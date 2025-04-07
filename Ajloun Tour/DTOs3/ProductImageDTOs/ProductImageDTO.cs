using Ajloun_Tour.Models;

namespace Ajloun_Tour.DTOs3.ProductImageDTOs
{
    public class ProductImageDTO
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string ImageURL { get; set; }
        public bool IsThumbnail { get; set; }
    }
}
