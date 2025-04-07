using Ajloun_Tour.DTOs3.CategoryDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class CategoryProducts : ICategoryProducts
    {
        private readonly MyDbContext _context;

        public CategoryProducts(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoryProductsAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            return categories.Select(c => new CategoryDTO
            {

                CategoryId = c.CategoryId,
                Name = c.Name,
            });
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) {

                throw new Exception($"Category With {id} IS Not Found"); 
            };

            return new CategoryDTO { 
            
                CategoryId = category.CategoryId,
                Name = category.Name,
            };
        }

        public async Task<CategoryDTO> AddCategoryAsync(CreateCategory createCategory)
        {
            var newCategory = new Category { 
            
                Name = createCategory.Name,
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return new CategoryDTO { 
            
                Name = createCategory.Name
            };
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategory updateCategory)
        {
            var updatedCategory = await _context.Categories.FindAsync(id);
            if (updatedCategory == null) {

                throw new Exception($"Category With This {id} Is Not Defined");

                updatedCategory.Name = updateCategory.Name ?? updatedCategory.Name;
            };
            _context.Categories.Update(updatedCategory);
            await _context.SaveChangesAsync();

            return new CategoryDTO { 
            
                CategoryId= updatedCategory.CategoryId,
                Name = updatedCategory.Name,
            };
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var deletedCategory = await _context.Categories.FindAsync(id);

            if (deletedCategory == null) {
            
                throw new Exception($"Category With This {id} Is Not Defined");
            };
        }
    }
}
