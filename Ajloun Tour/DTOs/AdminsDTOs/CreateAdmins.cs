using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.DTOs.AdminsDTOs
{
    public class CreateAdmins
    {
        [FromForm]
        public string Password { get; set; } = null!;
        [FromForm]
        public string Email { get; set; } = null!;
        [FromForm]
        public string FullName { get; set; } = null!;
        [FromForm]
        public IFormFile? AdminImage { get; set; }
    }
}
