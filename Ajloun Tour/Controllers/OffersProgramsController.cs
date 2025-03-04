using Ajloun_Tour.DTOs2.OffersProgramDTOs;
using Ajloun_Tour.DTOs2.OffersProgramDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersProgramsController : ControllerBase
    {
        private readonly IOfferProgramServiceRepository _offerProgramServiceRepository;

        public OffersProgramsController(IOfferProgramServiceRepository offerProgramServiceRepository)
        {
            _offerProgramServiceRepository = offerProgramServiceRepository;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<OffersProgramDTO>>> GetOffersPrograms()
        {
            var offerPrograms = await _offerProgramServiceRepository.GetAllOfferProgramsAsync();
            return Ok(offerPrograms);
        }

        // GET: api/TourPrograms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OffersProgramDTO>> GetOfferProgram(int id)
        {
            var offerProgram = await _offerProgramServiceRepository.GetOfferProgramByIdAsync(id);

            if (offerProgram == null)
            {
                return NotFound();
            }

            return Ok(offerProgram);
        }

        // GET: api/TourPrograms/tour/5
        [HttpGet("offer/{offerId}")]
        public async Task<ActionResult<IEnumerable<OffersProgramDTO>>> GetOfferProgramsByTourId(int offerId)
        {
            var offerPrograms = await _offerProgramServiceRepository.GetOfferProgramsByOfferIdAsync(offerId);
            return Ok(offerPrograms);
        }

        // GET: api/TourPrograms/tour/5/programs
        [HttpGet("offer/{offerId}/Withprograms")]
        public async Task<ActionResult<OfferWithProgramsDTO>> GetTourWithPrograms(int offerId)
        {
            var offerWithPrograms = await _offerProgramServiceRepository.GetOfferWithProgramsAsync(offerId);

            if (offerWithPrograms == null)
            {
                return NotFound($"Offer with ID {offerId} not found");
            }

            return Ok(offerWithPrograms);
        }

        // POST: api/TourPrograms
        [HttpPost]
        public async Task<ActionResult<OffersProgramDTO>> CreateOfferProgram([FromForm] CreateOffersProgram createOffersProgram)
        {
            var offerProgram = await _offerProgramServiceRepository.CreateOfferProgramAsync(createOffersProgram);
            return CreatedAtAction(nameof(GetOfferProgram), new { id = offerProgram.OfferProgramId }, offerProgram);
        }

        // PUT: api/TourPrograms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOfferProgram(int id, [FromForm] UpdateOfferProgramDTO updateOfferProgramDTO)
        {
            var offerProgram = await _offerProgramServiceRepository.UpdateOfferProgramAsync(id, updateOfferProgramDTO);

            if (offerProgram == null)
            {
                return NotFound();
            }

            return Ok(offerProgram);
        }

        // DELETE: api/TourPrograms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfferProgram(int id)
        {
            var result = await _offerProgramServiceRepository.DeleteOfferProgramAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
