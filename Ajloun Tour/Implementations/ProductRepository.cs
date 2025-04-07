using Ajloun_Tour.DTOs3.ProductDTOs;
using Ajloun_Tour.DTOs3.ProductImageDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;

namespace Ajloun_Tour.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_DIRECTORY = "ProductsImages";

        public ProductRepository(MyDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;

            // Ensure upload directory exists
            var uploadPath = Path.Combine(_environment.WebRootPath, UPLOAD_DIRECTORY);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
        }



        private async Task<string> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentException("Invalid image file");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var relativePath = Path.Combine(UPLOAD_DIRECTORY, fileName);
            var absolutePath = Path.Combine(_environment.WebRootPath, relativePath);

            using (var stream = new FileStream(absolutePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/{relativePath.Replace('\\', '/')}";
        }

        private void DeleteImageFile(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl)) return;

                var absolutePath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
                if (File.Exists(absolutePath))
                {
                    File.Delete(absolutePath);
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw - this is cleanup code
                // Consider adding proper logging here
                Console.WriteLine($"Error deleting image file: {ex.Message}");
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProducts(bool includeInactive = false)
        {
            try
            {
                IQueryable<Product> query = _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .AsNoTracking(); // Add this for better performance in read-only scenarios

                if (!includeInactive)
                {
                    query = query.Where(p => p.Status == true);
                }

                var products = await query.ToListAsync();
                return _mapper.Map<IEnumerable<ProductDTO>>(products);
            }
            catch (Exception ex)
            {
                // Add proper logging here
                throw;
            }
        }
        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)  // Include images
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                throw new NotFoundException($"Product with ID {id} not found");

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> AddProduct(CreateProductDTO createProduct)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = _mapper.Map<Product>(createProduct);

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                if (createProduct.Images != null && createProduct.Images.Any())
                {
                    foreach (var image in createProduct.Images)
                    {
                        var imageUrl = await SaveImage(image);
                        var productImage = new ProductImage
                        {
                            ProductId = product.ProductId,
                            ImageUrl = imageUrl,
                            IsThumbnail = !product.ProductImages.Any() // First image as thumbnail
                        };
                        _context.ProductImages.Add(productImage);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                // Reload the product with images
                var productWithImages = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

                return _mapper.Map<ProductDTO>(productWithImages);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<ProductImageDTO> AddProductImage(int productId, IFormFile image, bool isThumbnail = false)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                throw new NotFoundException($"Product with ID {productId} not found");

            // If this is set as thumbnail, remove thumbnail flag from other images
            if (isThumbnail)
            {
                foreach (var existingImage in product.ProductImages)
                {
                    existingImage.IsThumbnail = false;
                }
            }

            // If this is the first image, make it thumbnail by default
            if (!product.ProductImages.Any())
            {
                isThumbnail = true;
            }

            var imageUrl = await SaveImage(image);

            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = imageUrl,
                IsThumbnail = isThumbnail
            };

            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductImageDTO>(productImage);
        }

        public async Task DeleteProductImage(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new NotFoundException($"Image with ID {imageId} not found");

            // Delete physical file
            var fullPath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductImageDTO>> GetProductImages(int productId)
        {
            var images = await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductImageDTO>>(images);
        }
        public async Task SetThumbnailImage(int productId, int imageId)
        {
            var images = await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();

            foreach (var image in images)
            {
                image.IsThumbnail = (image.ImageId == imageId);
            }

            await _context.SaveChangesAsync();
        }

        // Update the UpdateProduct method to handle images
        public async Task<ProductDTO> UpdateProduct(int id, UpdateProductDTO updateProduct)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                    throw new NotFoundException($"Product with ID {id} not found");

                _mapper.Map(updateProduct, product);
                product.UpdatedAt = DateTime.UtcNow;

                // Handle image deletions
                if (updateProduct.ImagesToDelete?.Any() == true)
                {
                    var imagesToDelete = product.ProductImages
                        .Where(pi => updateProduct.ImagesToDelete.Contains(pi.ImageId))
                        .ToList();

                    foreach (var image in imagesToDelete)
                    {
                        DeleteImageFile(image.ImageUrl);
                        _context.ProductImages.Remove(image);
                    }
                }

                // Handle new images
                if (updateProduct.NewImages?.Any() == true)
                {
                    bool needsThumbnail = !product.ProductImages.Any(pi => pi.IsThumbnail == true);
                    foreach (var image in updateProduct.NewImages)
                    {
                        await AddProductImage(id, image, isThumbnail: needsThumbnail);
                        needsThumbnail = false;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Refresh the product from database to get updated images
                await _context.Entry(product).ReloadAsync();
                return _mapper.Map<ProductDTO>(product);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteProduct(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                    throw new NotFoundException($"Product with ID {id} not found");

                // Delete associated image files
                foreach (var image in product.ProductImages)
                {
                    DeleteImageFile(image.ImageUrl);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategory(int categoryId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.Status == true)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

    }
}
