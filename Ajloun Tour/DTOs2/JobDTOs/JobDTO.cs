namespace Ajloun_Tour.DTOs2.JobDTOs
{
    public class JobDTO
    {
        public int JobId { get; set; }
        public string? Title { get; set; }
        public string? JobType { get; set; }
        public string? Salary { get; set; }
        public int? Vacancies { get; set; }
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? Overview { get; set; }
        public string? Experinces { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string MainImage { get; set; } 
        public List<string> SubImages { get; set; }









    }
}
