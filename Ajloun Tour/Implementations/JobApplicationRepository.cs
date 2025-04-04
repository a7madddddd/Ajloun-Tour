using Ajloun_Tour.DTOs2.JobApplicationDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;

namespace Ajloun_Tour.Implementations
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<JobApplicationRepository> _logger;

        public JobApplicationRepository(
            MyDbContext context,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            ILogger<JobApplicationRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<IEnumerable<JobApplicationDTO>> GetAllApplications()
        {
            var applications = await _context.JobApplications
                .Include(a => a.Job)
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobApplicationDTO>>(applications);
        }

        public async Task<JobApplicationDTO> GetApplicationById(int id)
        {
            var application = await _context.JobApplications
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
                throw new NotFoundException($"Application with ID {id} not found");

            return _mapper.Map<JobApplicationDTO>(application);
        }

        public async Task<IEnumerable<JobApplicationDTO>> GetApplicationsByJobId(int jobId)
        {
            var applications = await _context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobApplicationDTO>>(applications);
        }

        public async Task<IEnumerable<JobApplicationDTO>> GetApplicationsByEmail(string email)
        {
            var applications = await _context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.Email == email)
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobApplicationDTO>>(applications);
        }

        public async Task<JobApplicationDTO> AddApplication(CreateApplication createApplication)
        {
            try
            {
                // Verify job exists
                var job = await _context.Jobs
                    .FirstOrDefaultAsync(j => j.JobId == createApplication.JobId);

                if (job == null)
                    throw new NotFoundException($"Job with ID {createApplication.JobId} not found");

                //if (job.IsActive ==false)
                //    throw new BadHttpRequestException("This job is no longer accepting applications");

                // Create new application
                var application = new JobApplication
                {
                    JobId = createApplication.JobId,
                    ApplicantName = createApplication.ApplicantName,
                    Email = createApplication.Email,
                    Phone = createApplication.Phone,
                    Message = createApplication.Message,
                    ApplicationDate = DateTime.UtcNow,
                    Status = "Pending",
                    Cvpath = null // Default to null
                };

                // Only process CV if it exists
                if (createApplication.CV != null && createApplication.CV.Length > 0)
                {
                    application.Cvpath = await SaveCV(createApplication.CV);
                }

                _context.JobApplications.Add(application);
                await _context.SaveChangesAsync();

                // Map to DTO and return
                return new JobApplicationDTO
                {
                    ApplicationId = application.ApplicationId,
                    JobId = application.JobId ?? job.JobId,
                    ApplicantName = application.ApplicantName,
                    Email = application.Email,
                    Phone = application.Phone,
                    Message = application.Message,
                    CVPath = application.Cvpath,
                    ApplicationDate = application.ApplicationDate.GetValueOrDefault(),
                    Status = application.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddApplication: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<JobApplicationDTO> UpdateApplicationStatus(int id, string status)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
                throw new NotFoundException($"Application with ID {id} not found");

            // Validate status
            var validStatuses = new[] { "Pending", "Reviewed", "Shortlisted", "Rejected", "Accepted" };
            if (!validStatuses.Contains(status))
                throw new Exception($"Invalid status. Valid statuses are: {string.Join(", ", validStatuses)}");

            application.Status = status;
            await _context.SaveChangesAsync();

            return await GetApplicationById(id);
        }

        public async Task DeleteApplication(int id)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
                throw new NotFoundException($"Application with ID {id} not found");

            // Delete CV file
            DeleteCV(application.Cvpath);

            _context.JobApplications.Remove(application);
            await _context.SaveChangesAsync();
        }

        private async Task<string> SaveCV(IFormFile cv)
        {
            if (cv == null || cv.Length == 0)
                throw new Exception("Invalid CV file");

            // Validate file type
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(cv.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new Exception("Invalid file type. Only PDF and Word documents are allowed.");

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "cvs");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await cv.CopyToAsync(fileStream);
            }

            return $"/cvs/{fileName}";
        }

        private void DeleteCV(string cvPath)
        {
            if (string.IsNullOrEmpty(cvPath))
                return;

            var fileName = Path.GetFileName(cvPath);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "cvs", fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
