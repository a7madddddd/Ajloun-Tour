using Ajloun_Tour.DTOs3.ProductDTOs;
using Ajloun_Tour.DTOs3.ProductImageDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts([FromQuery] bool includeInactive = false)
        {
            var products = await _productRepository.GetAllProducts(includeInactive);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromForm] CreateProductDTO createProduct)
        {
            var product = await _productRepository.AddProduct(createProduct);
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, UpdateProductDTO updateProduct)
        {
            var product = await _productRepository.UpdateProduct(id, updateProduct);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
            return NoContent();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategory(categoryId);
            return Ok(products);
        }


        [HttpPost("{productId}/images")]
        public async Task<ActionResult<ProductImageDTO>> AddProductImage(
    int productId,
    IFormFile image,
    [FromQuery] bool isThumbnail = false)
        {
            var productImage = await _productRepository.AddProductImage(productId, image, isThumbnail);
            return Ok(productImage);
        }

        [HttpDelete("images/{imageId}")]
        public async Task<ActionResult> DeleteProductImage(int imageId)
        {
            await _productRepository.DeleteProductImage(imageId);
            return NoContent();
        }

        [HttpPut("{productId}/images/{imageId}/thumbnail")]
        public async Task<ActionResult> SetThumbnail(int productId, int imageId)
        {
            await _productRepository.SetThumbnailImage(productId, imageId);
            return NoContent();
        }
    }
}
