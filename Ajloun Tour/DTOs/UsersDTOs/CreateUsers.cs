namespace Ajloun_Tour.DTOs.UsersDTOs
{
    public class CreateUsers
    {
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public FormFile? ImageFile { get; set; }
    }
}
