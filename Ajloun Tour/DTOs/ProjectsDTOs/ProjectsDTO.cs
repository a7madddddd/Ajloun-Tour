using Ajloun_Tour.Models;

namespace Ajloun_Tour.DTOs.ProjectsDTOs
{
    public class ProjectsDTO
    {
        public int ProjectId { get; set; }
        public string? Status { get; set; }
        public string? ProjectImage { get; set; }
        public int? AdminId { get; set; }

    }
}
