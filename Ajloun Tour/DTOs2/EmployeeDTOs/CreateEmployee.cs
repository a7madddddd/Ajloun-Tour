namespace Ajloun_Tour.DTOs2.EmployeeDTOs
{
    public class CreateEmployee
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
