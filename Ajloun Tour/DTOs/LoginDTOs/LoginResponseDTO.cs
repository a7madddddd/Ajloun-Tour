namespace Ajloun_Tour.DTOs.LoginDTOs
{
    public class LoginResponseDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserImage { get; set; }
        public string Token { get; set; }
    }
}
