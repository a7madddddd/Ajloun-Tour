namespace Ajloun_Tour.DTOs3.ProductDTOs
{
    public class UpdateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int CategoryId { get; set; }
        public string Weight { get; set; }
        public string Dimensions { get; set; }
        public string Tag { get; set; }
        public int Limit { get; set; }
        public string Status { get; set; }
        public List<IFormFile> NewImages { get; set; }  // For adding new images
        public List<int> ImagesToDelete { get; set; }   // For removing existing images
    }
}
