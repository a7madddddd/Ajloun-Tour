using Ajloun_Tour.DTOs.NewsLattersDTO;
using Ajloun_Tour.DTOs.OffersDTOs;
using Ajloun_Tour.Implementations;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IOffersRepository _offersRepository;

        public OffersController(IOffersRepository offersRepository)
        {
            _offersRepository = offersRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OffersDTO>>> GetAllOfferss()
        {

            var allOffers = await _offersRepository.GetAllOffersAsync();
            return Ok(allOffers);
        }

        [HttpGet("id")]
        public async Task<ActionResult<OffersDTO>> GetOfferById(int id)
        {

            var Offer = await _offersRepository.GetOffersById(id);
            return Ok(Offer);
        }

        [HttpPost]
        public async Task<ActionResult<OffersDTO>> AddOffersAsync([FromForm] CreateOffers createOffers)
        {

            var AddOffer = await _offersRepository.AddOffersAsync(createOffers);
            return Ok(AddOffer);
        }
        [HttpPut("id")]
        public async Task<ActionResult<OffersDTO>> updateOffersAsync(int id, [FromBody] CreateOffers createOffers)
        {

            var updatedOffer = await _offersRepository.UpdateOffersAsync(id, createOffers);
            return Ok(updatedOffer);
        }
        [HttpDelete("id")]
        public async void DeleteOffersAsync(int id)
        {
            await _offersRepository.DeleteOffersAsync(id);

        }
    }
}
