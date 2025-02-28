using Ajloun_Tour.DTOs.AdminsDTOs;
using Ajloun_Tour.DTOs.ToursDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursController : ControllerBase
    {
        private readonly IToursRepository _toursRepository;
        private readonly MyDbContext _context;

        public ToursController(IToursRepository toursRepository, MyDbContext context)
        {
            _toursRepository = toursRepository;
            _context = context;
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

        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<ToursDTO>> AddToursAsync([FromForm] CreateTours createTours)
        {
            if (createTours.TourImage == null)
            {
                return BadRequest("Tour image is required.");
            }

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


        [HttpGet("LastFourToursWithRatings")]
        public async Task<ActionResult<IEnumerable<TourWithRatingDTO>>> GetLastFourToursWithRatings()
        {
            var tours = await _context.Tours
                .OrderByDescending(t => t.TourId)
                .Take(4)
                .Select(t => new TourWithRatingDTO
                {
                    TourId = t.TourId,
                    TourName = t.TourName,
                    Description = t.Description,
                    Price = t.Price,
                    Duration = t.Duration,
                    IsActive = (bool)t.IsActive,
                    TourImage = t.TourImage,
                    AverageRating = t.Reviews.Any()
                        ? t.Reviews.Average(r => r.Rating)
                        : null
                })
                .ToListAsync();

            return Ok(tours);
        }
    }
}


