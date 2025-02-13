namespace Ajloun_Tour.DTOs.AdminsDTOs
{
    public class AdminsDTO
    {
        public int AdminId { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? AdminImage { get; set; }
    }
}
