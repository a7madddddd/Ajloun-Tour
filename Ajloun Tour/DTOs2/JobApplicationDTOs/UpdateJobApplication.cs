using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs2.JobApplicationDTOs
{
    public class UpdateJobApplication
    {
        [Required]
        public string Status { get; set; }
    }
}
