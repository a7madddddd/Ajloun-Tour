using Ajloun_Tour.DTOs.TestoDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestomonialsController : ControllerBase
    {
        private readonly ITestomonialsRepository _testomonialsRepository;

        public TestomonialsController(ITestomonialsRepository testomonialsRepository)
        {
            _testomonialsRepository = testomonialsRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestoDTO>>> GetAllTestoAsync()
        {

            var testo = await _testomonialsRepository.GetAllTestoAsync();
            return Ok(testo);
        }

        [HttpGet("id")]
        public async Task<ActionResult<TestoDTO>> GetTestoById(int id) {
        
            var test = await _testomonialsRepository.GetTestoById(id);
            return Ok(test);
        }

        [HttpPost]
        public async Task<ActionResult<TestoDTO>> AddNewTestoAsync(CreateTesto createTesto) {
        
            var newTesto = await _testomonialsRepository.AddTestoAsync( createTesto );
            return Ok( newTesto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateTestoDTO>> UpdateTesto(int id, UpdateTestoDTO updateTestoDto)
        {
            try
            {
                var updatedTesto = await _testomonialsRepository.UpdateTestoById(id, updateTestoDto);
                return Ok(updatedTesto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the testimonial.", error = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTestoAsync(int id)
        {
            try
            {
                await _testomonialsRepository.DeleteTestoAsync(id); // Make sure to await the method here
                return Ok(new { message = $"Testimonial with ID {id} deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // Return NotFound if the testimonial is not found
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the testimonial.", error = ex.Message });
            }
        }

    }
}
