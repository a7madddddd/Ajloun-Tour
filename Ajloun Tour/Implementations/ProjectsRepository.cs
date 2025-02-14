using Ajloun_Tour.DTOs.ProjectsDTOs;
using Ajloun_Tour.DTOs.ToursDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly MyDbContext _context;
        private readonly string _imageDirectory;

        public ProjectsRepository(MyDbContext context)
        {

            _context = context;
            _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProjectsImages");
            EnsureImageDirectoryExists();
        }

        private void EnsureImageDirectoryExists()
        {
            if (!Directory.Exists(_imageDirectory))
            {
                Directory.CreateDirectory(_imageDirectory);
            }
        }

        private async Task<string> SaveImageFileAsync(IFormFile imageFile)
        {
            string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            string filePath = Path.Combine(_imageDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return fileName;
        }


        private async Task DeleteImageFileAsync(string fileName)
        {
            string filePath = Path.Combine(_imageDirectory, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }





        public async Task<IEnumerable<ProjectsDTO>> GetAllProjects()
        {
            var prjects = await _context.Projects.ToListAsync();

            return prjects.Select(p => new ProjectsDTO
            {

                ProjectId = p.ProjectId,
                AdminId = p.AdminId,
                ProjectName = p.ProjectName,
                Status = p.Status,
                ProjectImage = p.ProjectImage
            });
        }

        public async Task<ProjectsDTO> GetProjectById(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {

                throw new Exception("This Project Is Not Defined");
            }

            return new ProjectsDTO
            {

                ProjectId = project.ProjectId,
                AdminId = project.AdminId,
                ProjectName = project.ProjectName,
                Status = project.Status,
                ProjectImage = project.ProjectImage
            };
        }

        public async Task<ProjectsDTO> AddProjectAsync(CreateProjects createProjects)
        {
            if (createProjects.ProjectImage == null)
            {
                throw new ArgumentNullException(nameof(createProjects.ProjectImage), "Image file is required.");
            }

            var fileName = await SaveImageFileAsync(createProjects.ProjectImage);

            var project = new Project
            {
                AdminId = createProjects.AdminId,
                ProjectName = createProjects.ProjectName,
                Status = createProjects.Status,
                ProjectImage = fileName
            };


            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return new ProjectsDTO
            {
                ProjectId = project.ProjectId,
                AdminId = project.AdminId,
                ProjectName = project.ProjectName,
                Status = project.Status,
                ProjectImage = fileName

            };
        }

        public async Task<ProjectsDTO> UpdateProjectAsync(int id, CreateProjects createProjects)
        {
            var updatedProject = await _context.Projects.FindAsync(id);

            if (updatedProject == null)
            {

                throw new ArgumentNullException(nameof(updatedProject));
            }

            updatedProject.AdminId = createProjects.AdminId ?? updatedProject.AdminId;
            updatedProject.ProjectName = createProjects.ProjectName?? updatedProject.ProjectName;
            updatedProject.Status = createProjects.Status ?? updatedProject.Status;



            if (createProjects.ProjectImage != null)
            {
                if (!string.IsNullOrEmpty(updatedProject.ProjectImage))
                {
                    await DeleteImageFileAsync(updatedProject.ProjectImage);
                }
                var fileName = await SaveImageFileAsync(createProjects.ProjectImage);
                updatedProject.ProjectImage = fileName;
            }

            _context.Projects.Update(updatedProject);
            await _context.SaveChangesAsync();

            return new ProjectsDTO
            {
                ProjectId = updatedProject.ProjectId,
                AdminId = updatedProject.AdminId,
                ProjectName = updatedProject.ProjectName,
                Status = updatedProject.Status,
                ProjectImage = updatedProject.ProjectImage,

            };
        }

        public async Task DeleteProjectAsync(int id)
        {
            var deletedProject = await _context.Projects.FindAsync(id);

            if (deletedProject == null)
            {

                throw new ArgumentNullException(nameof(deletedProject));
            }

            _context.Projects.Remove(deletedProject);
            await _context.SaveChangesAsync();
        }
    }
}
