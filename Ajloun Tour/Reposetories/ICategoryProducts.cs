using Ajloun_Tour.DTOs3.CategoryDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface ICategoryProducts
    {
        Task<IEnumerable<CategoryDTO>> GetCategoryProductsAsync();
        Task<CategoryDTO> GetCategoryById(int id);
        Task<CategoryDTO> AddCategoryAsync(CreateCategory createCategory);
        Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategory updateCategory);
        Task DeleteCategoryAsync(int id);

    }
}
