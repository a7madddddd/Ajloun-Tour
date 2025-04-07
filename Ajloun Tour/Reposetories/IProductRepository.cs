using Ajloun_Tour.DTOs3.ProductDTOs;
using Ajloun_Tour.DTOs3.ProductImageDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetAllProducts(bool includeInactive = false);
        Task<ProductDTO> GetProductById(int id);
        Task<ProductDTO> AddProduct(CreateProductDTO createProduct);
        Task<ProductDTO> UpdateProduct(int id, UpdateProductDTO updateProduct);
        Task DeleteProduct(int id);
        Task<IEnumerable<ProductDTO>> GetProductsByCategory(int categoryId);

        Task<ProductImageDTO> AddProductImage(int productId, IFormFile image, bool isThumbnail = false);
        Task DeleteProductImage(int imageId);
        Task<IEnumerable<ProductImageDTO>> GetProductImages(int productId);
        Task SetThumbnailImage(int productId, int imageId);

    }
}
