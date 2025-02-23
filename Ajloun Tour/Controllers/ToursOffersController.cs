using Ajloun_Tour.DTOs.ToursOffersDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursOffersController : ControllerBase
    {
        private readonly IToursOffersRepository _toursOffersRepository;

        public ToursOffersController(IToursOffersRepository toursOffersRepository)
        {
            _toursOffersRepository = toursOffersRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToursOffersDTO>>> GetAllOffersAsync()
        {


            var ToursOffers = await _toursOffersRepository.GetAllToursOffers();

            return Ok(ToursOffers);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ToursOffersDTO>> GetTourOfferAsync(int tourId, int offerId)
        {

            var tourOffer = await _toursOffersRepository.GetTourOfferById(tourId, offerId);
            return Ok(tourOffer);
        }

        [HttpGet("Activate")]
        public async Task<ActionResult<ToursOffersDTO>> GetActivateToursOffers()
        {

            var activateOffers = await _toursOffersRepository.GetActiveOffers();
            return Ok(activateOffers);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateTourOffers(TourOffer tourOffer)
        {
            try
            {
                var result = await _toursOffersRepository.AddTourOffer(tourOffer);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(false);
            }
            catch (Exception)
            {
                return StatusCode(500, false);
            }
        }


        [HttpPut("{tourId}")]
        public async Task<ActionResult<ToursOffersDTO>> Update(int tourId, CreateToursOffer createToursOffer)
        {
            try
            {
                var result = await _toursOffersRepository.UpdateTourOffer(tourId, createToursOffer);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpDelete("{tourId}/{offerId}")]
        public async Task<ActionResult<bool>> DeleteToursOffers(int tourId, int offerId)
        {
            try
            {
                var result = await _toursOffersRepository.DeleteTourOffer(tourId, offerId);
                if (!result)
                    return NotFound(false);

                return Ok(true);
            }
            catch (Exception)
            {
                return StatusCode(500, false);
            }
        }
    }
}
