using Ajloun_Tour.DTOs3.CategoryDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryProducts _categoryProducts;

        public CategoriesController(ICategoryProducts categoryProducts)
        {
            _categoryProducts = categoryProducts;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {

            var Categories = await _categoryProducts.GetCategoryProductsAsync();

            return Ok(Categories);
        }

        [HttpGet("id")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {

            var Category = await _categoryProducts.GetCategoryById(id);
            return Ok(Category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> AddNewCategory(CreateCategory createCategory)
        {

            var newCategory = await _categoryProducts.AddCategoryAsync(createCategory);
            return Ok(newCategory);
        }

        [HttpPut("id")]
        public async Task <ActionResult<CategoryDTO>> UpdateCategoryAsync(int id, UpdateCategory updateCategory)
        {

            var UpdatedCategory = await _categoryProducts.UpdateCategoryAsync(id, updateCategory);

            return Ok(UpdatedCategory);
        }

        [HttpDelete("Id")]
        public async Task<ActionResult> DeleteCategoryAsync(int id) {

            var DeletedCategory =  _categoryProducts.DeleteCategoryAsync(id);

            return Ok(DeletedCategory);
        }
    }
}
