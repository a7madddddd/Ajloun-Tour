using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs.LoginDTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
