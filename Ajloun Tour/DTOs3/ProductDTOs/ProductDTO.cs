using Ajloun_Tour.DTOs3.ProductImageDTOs;

namespace Ajloun_Tour.DTOs3.ProductDTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } // Add this instead of whole Category object
        public string Weight { get; set; }
        public string Dimensions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Tag { get; set; }
        public int Limit { get; set; }
        public bool Status { get; set; }
        public List<ProductImageDTO> Images { get; set; } = new List<ProductImageDTO>();
    }
}
