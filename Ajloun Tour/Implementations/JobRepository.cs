using Ajloun_Tour.DTOs2.JobDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    // Repositories/JobRepository.cs
    public class JobRepository : IJobRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JobRepository(MyDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new Exception("Invalid image file");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "JobsImages");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return $"/images/jobs/{fileName}";
        }


        private void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            var fileName = Path.GetFileName(imageUrl);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "JobsImages", fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }


        public async Task<IEnumerable<JobDTO>> GetAllJobs(bool includeInactive = false)
        {
            var query = _context.Jobs
                .Include(j => j.JobImages)
                .AsQueryable();
            
            var jobs = await query.OrderByDescending(j => j.CreatedDate).ToListAsync();
            return _mapper.Map<IEnumerable<JobDTO>>(jobs);
        }

        public async Task<JobDTO> GetJobById(int id)
        {
            var job = await _context.Jobs
                .Include(j => j.JobImages)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
                throw new Exception($"Job with ID {id} not found");

            return _mapper.Map<JobDTO>(job);
        }

        public async Task<IEnumerable<JobDTO>> GetJobsByType(string jobType)
        {
            var jobs = await _context.Jobs
                .Include(j => j.JobImages)
                .Where(j => j.JobType == jobType )
                .OrderByDescending(j => j.CreatedDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobDTO>>(jobs);
        }

        public async Task<JobDTO> AddJob(CreateJobDTO createJob)
        {
            var job = _mapper.Map<Job>(createJob);
            job.CreatedDate = DateTime.UtcNow;
            job.IsActive = false;

            // Handle image uploads
            if (createJob.Images != null && createJob.Images.Any())
            {
                job.JobImages = new List<JobImage>();
                foreach (var imageFile in createJob.Images)
                {
                    var imageUrl = await SaveImage(imageFile);
                    job.JobImages.Add(new JobImage { ImageUrl = imageUrl });
                }
            }

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return await GetJobById(job.JobId);
        }

        public async Task<JobDTO> UpdateJob(int id, UpdateJobDTO updateJob)
        {
            var existingJob = await _context.Jobs
                .Include(j => j.JobImages)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (existingJob == null)
                throw new Exception($"Job with ID {id} not found");

            _mapper.Map(updateJob, existingJob);

            // Handle image uploads
            if (updateJob.Images != null && updateJob.Images.Any())
            {
                // Delete existing images
                foreach (var image in existingJob.JobImages)
                {
                    DeleteImage(image.ImageUrl);
                }
                existingJob.JobImages.Clear();

                // Add new images
                foreach (var imageFile in updateJob.Images)
                {
                    var imageUrl = await SaveImage(imageFile);
                    existingJob.JobImages.Add(new JobImage { ImageUrl = imageUrl });
                }
            }

            await _context.SaveChangesAsync();
            return await GetJobById(id);
        }

        public async Task DeleteJob(int id)
        {
            var job = await _context.Jobs
                .Include(j => j.JobImages)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
                throw new Exception($"Job with ID {id} not found");

            // Delete associated images
            foreach (var image in job.JobImages)
            {
                DeleteImage(image.ImageUrl);
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }


    }


}
