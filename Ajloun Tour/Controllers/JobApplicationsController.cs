using Ajloun_Tour.DTOs2.JobApplicationDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationRepository _applicationRepository;
        private readonly ILogger<JobApplicationsController> _logger;

        public JobApplicationsController(
            IJobApplicationRepository applicationRepository,
            ILogger<JobApplicationsController> logger)
        {
            _applicationRepository = applicationRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobApplicationDTO>>> GetApplications()
        {
            try
            {
                var applications = await _applicationRepository.GetAllApplications();
                return Ok(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all applications");
                return StatusCode(500, "An error occurred while retrieving applications");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplicationDTO>> GetApplication(int id)
        {
            try
            {
                var application = await _applicationRepository.GetApplicationById(id);
                return Ok(application);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting application with ID: {ApplicationId}", id);
                return StatusCode(500, "An error occurred while retrieving the application");
            }
        }

        [HttpGet("job/{jobId}")]
        public async Task<ActionResult<IEnumerable<JobApplicationDTO>>> GetApplicationsByJob(int jobId)
        {
            try
            {
                var applications = await _applicationRepository.GetApplicationsByJobId(jobId);
                return Ok(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting applications for job ID: {JobId}", jobId);
                return StatusCode(500, "An error occurred while retrieving applications");
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<JobApplicationDTO>> CreateApplication([FromForm] CreateApplication createApplication)
        {
            try
            {
                _logger.LogInformation("Attempting to create application for job {JobId}", createApplication.JobId);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state: {ModelState}",
                        string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                    return BadRequest(ModelState);
                }

                var application = await _applicationRepository.AddApplication(createApplication);

                _logger.LogInformation("Successfully created application {ApplicationId} for job {JobId}",
                    application.ApplicationId, application.JobId);

                return CreatedAtAction(
                    nameof(GetApplication),
                    new { id = application.ApplicationId },
                    application);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found error while creating application");
                return NotFound(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request error while creating application");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating application: {Message}", ex.Message);
                return StatusCode(500, $"An error occurred while creating the application: {ex.Message}");
            }
        }


        [HttpPut("{id}/status")]
        public async Task<ActionResult<JobApplicationDTO>> UpdateApplicationStatus(
        int id,
        [FromBody] UpdateJobApplication updateApplication)
        {
            try
            {
                var application = await _applicationRepository.UpdateApplicationStatus(id, updateApplication.Status);
                return Ok(application);
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
                _logger.LogError(ex, "Error updating application status for ID: {ApplicationId}", id);
                return StatusCode(500, "An error occurred while updating the application");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            try
            {
                await _applicationRepository.DeleteApplication(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting application with ID: {ApplicationId}", id);
                return StatusCode(500, "An error occurred while deleting the application");
            }
        }
    }
}
