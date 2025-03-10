namespace Ajloun_Tour.DTOs2.JobApplicationDTOs
{
    public class JobApplicationDTO
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public string? ApplicantName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CVPath { get; set; }
        public string? Message { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string? Status { get; set; }
        public string? Title { get; set; }

    }
}
