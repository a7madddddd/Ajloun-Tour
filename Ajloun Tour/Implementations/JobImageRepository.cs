using Ajloun_Tour.DTOs2.JobDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;

namespace Ajloun_Tour.Implementations
{
    // Repositories/JobImageRepository.cs
    public class JobImageRepository : IJobImageRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<JobImageRepository> _logger;

        public JobImageRepository(
            MyDbContext context,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            ILogger<JobImageRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<IEnumerable<JobImageDTO>> GetImagesByJobId(int jobId)
        {
            var images = await _context.JobImages
                .Where(i => i.JobId == jobId)
                .OrderByDescending(i => i.Job.CreatedDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobImageDTO>>(images);
        }

        public async Task<JobImageDTO> AddImage(int jobId, IFormFile image)
        {
            // Verify job exists
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                throw new NotFoundException($"Job with ID {jobId} not found");

            // Validate and save image
            var imageUrl = await SaveImage(image);

            var jobImage = new JobImage
            {
                JobId = jobId,
                ImageUrl = imageUrl,
            };

            _context.JobImages.Add(jobImage);
            await _context.SaveChangesAsync();

            return _mapper.Map<JobImageDTO>(jobImage);
        }

        public async Task DeleteImage(int imageId)
        {
            var image = await _context.JobImages.FindAsync(imageId);
            if (image == null)
                throw new NotFoundException($"Image with ID {imageId} not found");

            // Delete physical file
            DeleteImageFile(image.ImageUrl);

            _context.JobImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteImagesByJobId(int jobId)
        {
            var images = await _context.JobImages
                .Where(i => i.JobId == jobId)
                .ToListAsync();

            foreach (var image in images)
            {
                DeleteImageFile(image.ImageUrl);
                _context.JobImages.Remove(image);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new BadHttpRequestException("Invalid image file");

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                throw new BadHttpRequestException("Invalid file type. Only image files are allowed.");

            // Validate file size (e.g., max 5MB)
            if (image.Length > 5 * 1024 * 1024)
                throw new BadHttpRequestException("File size exceeds maximum limit of 5MB");

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "jobs");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return $"/images/jobs/{fileName}";
        }

        private void DeleteImageFile(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            var fileName = Path.GetFileName(imageUrl);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "wwwroot", "JobsImages", fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
