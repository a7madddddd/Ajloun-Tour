using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs2.JobApplicationDTOs
{
    public class CreateApplication
    {
        [Required]
        public int JobId { get; set; }

        [Required]
        [StringLength(100)]
        public string ApplicantName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public IFormFile CV { get; set; }

        public string Message { get; set; }
    }
}
