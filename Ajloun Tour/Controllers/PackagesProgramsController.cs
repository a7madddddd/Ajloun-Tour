using Ajloun_Tour.DTOs2.PackagesProgramDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesProgramsController : ControllerBase
    {
        private readonly IPackProgramServiceRepository _packageServiceRepository;

        public PackagesProgramsController(IPackProgramServiceRepository packageServiceRepository)
        {
            _packageServiceRepository = packageServiceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackagesProgramDTO>>> GetPackPrograms()
        {
            var packPrograms = await _packageServiceRepository.GetAllPackProgramsAsync();
            return Ok(packPrograms);
        }

        // GET: api/TourPrograms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PackagesProgramDTO>> GetPackProgram(int id)
        {
            var packProgram = await _packageServiceRepository.GetPackProgramByIdAsync(id);

            if (packProgram == null)
            {
                return NotFound();
            }

            return Ok(packProgram);
        }

        // GET: api/TourPrograms/tour/5
        [HttpGet("pack/{packId}")]
        public async Task<ActionResult<IEnumerable<PackagesProgramDTO>>> GetPackProgramsByTourId(int packId)
        {
            var packPrograms = await _packageServiceRepository.GetPackProgramsByPackIdAsync(packId);
            return Ok(packPrograms);
        }

        // GET: api/TourPrograms/tour/5/programs
        [HttpGet("pack/{packId}/Withprograms")]
        public async Task<ActionResult<PackageWithProgramsDTO>> GetTourWithPrograms(int packId)
        {
            var packWithPrograms = await _packageServiceRepository.GetPackProgramsByPackIdAsync(packId);

            if (packWithPrograms == null)
            {
                return NotFound($"Pack with ID {packId} not found");
            }

            return Ok(packWithPrograms);
        }

        // POST: api/TourPrograms
        [HttpPost]
        public async Task<ActionResult<PackagesProgramDTO>> CreatePackProgram([FromForm] CreatePackageProgram createPackageProgram)
        {
            var packProgram = await _packageServiceRepository.CreatePackProgramAsync(createPackageProgram);
            return CreatedAtAction(nameof(GetPackProgram), new { id = packProgram.PackageProgramId }, packProgram);
        }

        // PUT: api/TourPrograms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackProgram(int id, [FromForm] UpdatePackageProgramDTO updatePackageProgramDTO)
        {
            var tourProgram = await _packageServiceRepository.UpdatePackProgramAsync(id, updatePackageProgramDTO);

            if (tourProgram == null)
            {
                return NotFound();
            }

            return Ok(tourProgram);
        }

        // DELETE: api/TourPrograms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackProgram(int id)
        {
            var result = await _packageServiceRepository.DeletePackProgramAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
