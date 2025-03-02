using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Ajloun_Tour.DTOs.UsersDTOs
{
    public class CreateUsers
    {
        [FromForm]
        public string Password { get; set; } = null!;
        [FromForm]
        public string Email { get; set; } = null!;
        [FromForm]
        public string FullName { get; set; } = null!;
        [FromForm]
        public string? Phone { get; set; }
        [FromForm]
        public DateTime? CreatedAt { get; set; }
        [FromForm]
        public IFormFile? UserImage { get; set; }


    }
}
