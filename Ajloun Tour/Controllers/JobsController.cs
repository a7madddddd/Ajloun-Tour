using Ajloun_Tour.DTOs2.JobDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel;
using OpenQA.Selenium;
using System.IdentityModel;
namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;
        private readonly ILogger<JobsController> _logger;

        public JobsController(
            IJobRepository jobRepository,
            ILogger<JobsController> logger)
        {
            _jobRepository = jobRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetJobs([FromQuery] bool includeInactive = false)
        {
            try
            {
                var jobs = await _jobRepository.GetAllJobs(includeInactive);
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all jobs");
                return StatusCode(500, "An error occurred while retrieving jobs");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobDTO>> GetJob(int id)
        {
            try
            {
                var job = await _jobRepository.GetJobById(id);
                return Ok(job);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting job with ID: {JobId}", id);
                return StatusCode(500, "An error occurred while retrieving the job");
            }
        }

        [HttpGet("type/{jobType}")]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetJobsByType(string jobType)
        {
            try
            {
                var jobs = await _jobRepository.GetJobsByType(jobType);
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting jobs of type: {JobType}", jobType);
                return StatusCode(500, "An error occurred while retrieving jobs");
            }
        }

        [HttpPost]
        public async Task<ActionResult<JobDTO>> CreateJob([FromForm] CreateJobDTO createJob)
        {
            try
            {
                var job = await _jobRepository.AddJob(createJob);
                return CreatedAtAction(nameof(GetJob), new { id = job.JobId }, job);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new job");
                return StatusCode(500, "An error occurred while creating the job");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JobDTO>> UpdateJob(int id, [FromForm] UpdateJobDTO updateJob)
        {
            try
            {
                var job = await _jobRepository.UpdateJob(id, updateJob);
                return Ok(job);
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
                _logger.LogError(ex, "Error updating job with ID: {JobId}", id);
                return StatusCode(500, "An error occurred while updating the job");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            try
            {
                await _jobRepository.DeleteJob(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting job with ID: {JobId}", id);
                return StatusCode(500, "An error occurred while deleting the job");
            }
        }
    }
}
