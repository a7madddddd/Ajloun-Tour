using Ajloun_Tour.DTOs.OffersDTOs;
using Ajloun_Tour.DTOs2.ProgramDTOs;
using Ajloun_Tour.Implementations;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramRepository _programRepository;

        public ProgramsController(IProgramRepository programRepository)
        {
            _programRepository = programRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgramDTO>>> GetAllOPrograms()
        {

            var allPrograms = await _programRepository.GetAllPrograms();
            return Ok(allPrograms);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ProgramDTO>> GetProgramById(int id)
        {

            var program = await _programRepository.GetProgramById(id);
            return Ok(program);
        }

        [HttpPost]
        public async Task<ActionResult<ProgramDTO>> AddProgramsAsync([FromForm] CreateProgram createProgram)
        {

            var addProgram = await _programRepository.AddProgramAsync(createProgram);
            return Ok(addProgram);
        }
        [HttpPut("id")]
        public async Task<ActionResult<ProgramDTO>> updateProgramsAsync(int id, [FromForm] CreateProgram createProgram)
        {

            var updatedProgram = await _programRepository.UpdateProgramAsync(id, createProgram);
            return Ok(updatedProgram);
        }
        [HttpDelete("id")]
        public async void DeleteProgramsAsync(int id)
        {
            await _programRepository.DeleteProgramById(id);

        }
    }
}
