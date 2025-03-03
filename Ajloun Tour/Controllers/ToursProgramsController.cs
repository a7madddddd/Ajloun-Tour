using Ajloun_Tour.DTOs2.ToursProgramDTOs;
using Ajloun_Tour.Implementations;
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
        private readonly ITourProgramServiceRepository _tourProgramService;

        public ToursProgramsController(ITourProgramServiceRepository tourProgramService)
        {
            _tourProgramService = tourProgramService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToursProgramDTO>>> GetTourPrograms()
        {
            var tourPrograms = await _tourProgramService.GetAllTourProgramsAsync();
            return Ok(tourPrograms);
        }

        // GET: api/TourPrograms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToursProgramDTO>> GetTourProgram(int id)
        {
            var tourProgram = await _tourProgramService.GetTourProgramByIdAsync(id);

            if (tourProgram == null)
            {
                return NotFound();
            }

            return Ok(tourProgram);
        }

        // GET: api/TourPrograms/tour/5
        [HttpGet("tour/{tourId}")]
        public async Task<ActionResult<IEnumerable<ToursProgramDTO>>> GetTourProgramsByTourId(int tourId)
        {
            var tourPrograms = await _tourProgramService.GetTourProgramsByTourIdAsync(tourId);
            return Ok(tourPrograms);
        }

        // GET: api/TourPrograms/tour/5/programs
        [HttpGet("tour/{tourId}/Withprograms")]
        public async Task<ActionResult<TourWithProgramsDTO>> GetTourWithPrograms(int tourId)
        {
            var tourWithPrograms = await _tourProgramService.GetTourWithProgramsAsync(tourId);

            if (tourWithPrograms == null)
            {
                return NotFound($"Tour with ID {tourId} not found");
            }

            return Ok(tourWithPrograms);
        }

        // POST: api/TourPrograms
        [HttpPost]
        public async Task<ActionResult<ToursProgramDTO>> CreateTourProgram([FromForm] CreateToursProgram createTourProgramDTO)
        {
            var tourProgram = await _tourProgramService.CreateTourProgramAsync(createTourProgramDTO);
            return CreatedAtAction(nameof(GetTourProgram), new { id = tourProgram.TourProgramId }, tourProgram);
        }

        // PUT: api/TourPrograms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTourProgram(int id, [FromForm] UpdateTourProgramDTO updateTourProgramDTO)
        {
            var tourProgram = await _tourProgramService.UpdateTourProgramAsync(id, updateTourProgramDTO);

            if (tourProgram == null)
            {
                return NotFound();
            }

            return Ok(tourProgram);
        }

        // DELETE: api/TourPrograms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourProgram(int id)
        {
            var result = await _tourProgramService.DeleteTourProgramAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

