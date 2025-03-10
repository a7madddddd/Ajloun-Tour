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
        public async Task<ActionResult<JobApplicationDTO>> CreateApplication([FromForm] CreateApplication createApplication)
        {
            try
            {
                var application = await _applicationRepository.AddApplication(createApplication);
                return CreatedAtAction(nameof(GetApplication), new { id = application.ApplicationId }, application);
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
                _logger.LogError(ex, "Error creating new application");
                return StatusCode(500, "An error occurred while creating the application");
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
