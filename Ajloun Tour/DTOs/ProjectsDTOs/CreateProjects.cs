using Ajloun_Tour.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.DTOs.ProjectsDTOs
{
    public class CreateProjects
    {
        [FromForm]
        public string? Status { get; set; }
        [FromForm]
        public IFormFile? ProjectImage { get; set; }
        [FromForm]
        public int? AdminId { get; set; }
        [FromForm]
        public string? ProjectName { get; set; }


    }
}
