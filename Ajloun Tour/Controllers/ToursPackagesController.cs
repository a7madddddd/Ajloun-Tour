using Ajloun_Tour.DTOs.ToursPackagesDTOs;
using Ajloun_Tour.DTOs.ToursPackages;
using Ajloun_Tour.Implementations;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursPackagesController : ControllerBase
    {
        private readonly IToursPackagesRepository _toursPackagesRepository;

        public ToursPackagesController(IToursPackagesRepository toursPackagesRepository)
        {
            _toursPackagesRepository = toursPackagesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToursPackagesDTO>>> GetAllPackagesAsync()
        {


            var ToursPackage = await _toursPackagesRepository.GetAllToursPackages();

            return Ok(ToursPackage);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ToursPackagesDTO>> GetTourOfferAsync(int TourId, int PackageId)
        {

            var tourPackage = await _toursPackagesRepository.GetTourPackageById(TourId, PackageId);
            return Ok(tourPackage);
        }

        //[HttpGet("Activate")]
        //public async Task<ActionResult<ToursPackagesDTO>> GetActivateToursPackage()
        //{

        //    var activatePackage = await _toursPackagesRepository.GetActivePackage();
        //    return Ok(activatePackage);
        //}

        [HttpPost]
        public async Task<ActionResult<ToursPackagesDTO>> CreateTourPackage([FromForm]CreateToursPackages createToursPackages)
        {
            try
            {
                var result = await _toursPackagesRepository.AddTourPackage(createToursPackages);
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


        [HttpPut("{TourId}")]
        public async Task<ActionResult<ToursPackagesDTO>> UpdatePackage(int TourId,[FromBody] CreateToursPackages createToursPackages)
        {
            try
            {
                var result = await _toursPackagesRepository.UpdateTourPackages(TourId, createToursPackages);
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

        [HttpDelete("{TourId}/{PackageId}")]
        public async Task<ActionResult<bool>> DeleteToursPackage(int TourId, int PackageId)
        {
            try
            {
                var result = await _toursPackagesRepository.DeleteTourPackages(TourId, PackageId);
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
