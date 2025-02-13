using Ajloun_Tour.DTOs.ProjectsDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IProjectsRepository
    {
        Task<IEnumerable<ProjectsDTO>> GetAllProjects();

        Task<ProjectsDTO> GetProjectById(int id);

        Task<ProjectsDTO> AddProjectAsync(CreateProjects createProjects);
        Task<ProjectsDTO> UpdateProjectAsync(int id, CreateProjects createProjects);

        Task DeleteProjectAsync(int id);
    }
}
