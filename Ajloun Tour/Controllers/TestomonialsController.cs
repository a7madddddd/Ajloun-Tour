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

        [HttpDelete("id")]
        public async void DeleteTestoAsync(int id) {
        
            await _testomonialsRepository.DeleteTestoAsync( id );
            
        }
    }
}
