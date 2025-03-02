using Ajloun_Tour.DTOs2.ToursProgramDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursProgramsController : ControllerBase
    {
        private readonly IToursProgramRepository _toursProgramRepository;

        public ToursProgramsController(IToursProgramRepository toursProgramRepository)
        {
            _toursProgramRepository = toursProgramRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToursProgramDTO>>> GetToursPrograms()
        {
            var programs = await _toursProgramRepository.GetToursPrograms();
            return Ok(programs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToursProgramDTO>> GetTourProgram(int id)
        {
            var program = await _toursProgramRepository.GetTourProgram(id);
            if (program == null)
                return NotFound();

            return Ok(program);
        }

        [HttpGet("GetProgramByTourId")]
        public async Task<ActionResult<ToursProgramDTO>> GetProgramByTourId(int tourId)
        {
            var program = await _toursProgramRepository.GetProgramByTourId(tourId);

            if (program == null)
                return NotFound("No program found for this tour.");

            return Ok(program);
        }


        [HttpPost]
        public async Task<ActionResult<ToursProgramDTO>> CreateTourProgram(CreateToursProgram createToursProgram)
        {
            var program = await _toursProgramRepository.AddToursProgram(createToursProgram);
            return CreatedAtAction(nameof(GetTourProgram), new { id = program.ProgramId }, program);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToursProgramDTO>> UpdateTourProgram(int id, CreateToursProgram createToursProgram)
        {
            var program = await _toursProgramRepository.UpdateToursProgram(id, createToursProgram);
            if (program == null)
                return NotFound();

            return Ok(program);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourProgram(int id)
        {
            await _toursProgramRepository.DeleteToursProgram(id);
            return NoContent();
        }
    }
}
