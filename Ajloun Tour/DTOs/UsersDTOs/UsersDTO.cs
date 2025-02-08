namespace Ajloun_Tour.DTOs.UsersDTOs
{
    public class UsersDTO
    {
        public int UserId { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UserImage { get; set; }
    }
}
