using Ajloun_Tour.DTOs.ToursDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursController : ControllerBase
    {
        private readonly IToursRepository _toursRepository;

        public ToursController(IToursRepository toursRepository)
        {
            _toursRepository = toursRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToursDTO>>> GetAllToursAsync()
        {

            var Tours = await _toursRepository.GetAllToursAsync();
            return Ok(Tours);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ToursDTO>> GetTourById(int id)
        {

            var Tour = await _toursRepository.GetToursByIdAsync(id);
            return Ok(Tour);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ToursDTO>> AddToursAsync([FromForm] CreateTours createTours)
        {

            var newTour = await _toursRepository.AddToursAsync(createTours);
            return Ok(newTour);
        }

        [Authorize]
        [HttpPut("Id")]
        public async Task<ActionResult<ToursDTO>> UpdateToursAsync(int id, [FromBody] CreateTours createTours)
        {

            var updatedTour = await _toursRepository.UpdateToursAsync(id, createTours);
            return Ok(updatedTour);
        }

        [Authorize]
        [HttpDelete("Id")]
        public async void DeleteTourById(int id)
        {

            await _toursRepository.DeleteToursAsync(id);
            return;

        }
    }
}
