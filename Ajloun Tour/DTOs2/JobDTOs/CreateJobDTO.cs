using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs2.JobDTOs
{
    public class CreateJobDTO
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string JobType { get; set; }

        public string Salary { get; set; }

        [Range(1, 100)]
        public int Vacancies { get; set; }

        [Required]
        public string Description { get; set; }

        public string Requirements { get; set; }

        public List<IFormFile> Images { get; set; }
    }
}
