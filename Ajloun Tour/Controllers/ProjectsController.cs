using Ajloun_Tour.DTOs.ProjectsDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsRepository _projectsRepository;

        public ProjectsController(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectsDTO>>> GetAllProjectsAsync() {

            var Projects = await _projectsRepository.GetAllProjects();
            return Ok(Projects);
        }
        [HttpGet("id")]
        public async Task<ActionResult<ProjectsDTO>> GetProjectAsync(int id) {
        
            var project = await _projectsRepository.GetProjectById(id);
            return Ok(project);
        }
        [HttpPost]
        public async Task<ActionResult<ProjectsDTO>> AddProjectAsync([FromForm]CreateProjects createProjects) {
        
            var newProject = await _projectsRepository.AddProjectAsync(createProjects);
            return Ok(newProject);
        }
        [HttpPut("id")]
        public async Task<ActionResult<ProjectsDTO>> UpdateProjectAsync(int id, [FromBody]CreateProjects createProjects) {
        
            var UpdetedProject = await _projectsRepository.UpdateProjectAsync(id, createProjects);
            return Ok(UpdetedProject);
        }
        [HttpDelete("id")]
        public async void DeleteProjectAsync(int id) { 
        
            await _projectsRepository.DeleteProjectAsync(id);
        }
    }
}
