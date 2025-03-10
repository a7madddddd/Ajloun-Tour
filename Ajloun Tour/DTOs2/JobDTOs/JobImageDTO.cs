using Ajloun_Tour.Models;

namespace Ajloun_Tour.DTOs2.JobDTOs
{
    public class JobImageDTO
    {
        public int ImageId { get; set; }
        public int JobId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual Job Job { get; set; }
    }
}
