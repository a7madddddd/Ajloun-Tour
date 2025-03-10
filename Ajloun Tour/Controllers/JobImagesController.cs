using Ajloun_Tour.DTOs2.JobDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;

namespace Ajloun_Tour.Controllers
{
    // Controllers/JobImagesController.cs
    [ApiController]
    [Route("api/jobs/{jobId}/images")]
    public class JobImagesController : ControllerBase
    {
        private readonly IJobImageRepository _imageRepository;
        private readonly ILogger<JobImagesController> _logger;

        public JobImagesController(
            IJobImageRepository imageRepository,
            ILogger<JobImagesController> logger)
        {
            _imageRepository = imageRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobImageDTO>>> GetJobImages(int jobId)
        {
            try
            {
                var images = await _imageRepository.GetImagesByJobId(jobId);
                return Ok(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting images for job ID: {JobId}", jobId);
                return StatusCode(500, "An error occurred while retrieving images");
            }
        }

        [HttpPost]
        public async Task<ActionResult<JobImageDTO>> AddJobImage(
            int jobId,
            [FromForm] IFormFile image)
        {
            try
            {
                var jobImage = await _imageRepository.AddImage(jobId, image);
                return CreatedAtAction(nameof(GetJobImages), new { jobId }, jobImage);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding image for job ID: {JobId}", jobId);
                return StatusCode(500, "An error occurred while adding the image");
            }
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteJobImage(int jobId, int imageId)
        {
            try
            {
                await _imageRepository.DeleteImage(imageId);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image ID: {ImageId} for job ID: {JobId}", imageId, jobId);
                return StatusCode(500, "An error occurred while deleting the image");
            }
        }
    }
}
