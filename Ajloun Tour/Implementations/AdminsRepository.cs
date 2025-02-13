using Ajloun_Tour.DTOs.AdminsDTOs;
using Ajloun_Tour.DTOs.UsersDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ajloun_Tour.Implementations
{
    public class AdminsRepository : IAdminsRepository
    {
        private readonly MyDbContext _context;
        private readonly string _imageDirectory;
        private readonly IConfiguration _configuration;

        public AdminsRepository(MyDbContext context,  IConfiguration configuration)
        {
            _context = context;
            _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AdminsImages");
            EnsureImageDirectoryExists();
            _configuration = configuration;
        }


        private void EnsureImageDirectoryExists()
        {
            if (!Directory.Exists(_imageDirectory))
            {
                Directory.CreateDirectory(_imageDirectory);
            }
        }

        private async Task<string> SaveImageFileAsync(IFormFile imageFile)
        {
            string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            string filePath = Path.Combine(_imageDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return fileName;
        }


        private async Task DeleteImageFileAsync(string fileName)
        {
            string filePath = Path.Combine(_imageDirectory, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }


        public async Task<string> AuthenticateUserAsync(int id, string password)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.AdminId == id);

            if (admin == null)
            {
                return null; // User not found
            }

            var passwordHasher = new PasswordHasher<Admin>();
            var result = passwordHasher.VerifyHashedPassword(admin, admin.Password, password);

            if (result != PasswordVerificationResult.Success)
            {
                return null; // Invalid password
            }

            // Generate JWT Token
            return GenerateJwtToken(admin);
        }
        private string GenerateJwtToken(Admin admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, admin.AdminId.ToString()),
            new Claim(ClaimTypes.Name, admin.FullName),
            new Claim(ClaimTypes.Email, admin.Email)
        }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:ExpiryInHours"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }





        public async Task<IEnumerable<AdminsDTO>> GetAdminsAsync()
        {
            var admins = await _context.Admins.ToListAsync();

            return admins.Select(a => new AdminsDTO
            {

                AdminId = a.AdminId,
                FullName = a.FullName,
                Email = a.Email,
                Password = a.Password,
                AdminImage = a.AdminImage,
            });
        }

        public async Task<AdminsDTO> GetAdminById(int id)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
            {

                throw new Exception("This Admin Is Not Defined");
            };

            return new AdminsDTO
            {

                AdminId = admin.AdminId,
                FullName = admin.FullName,
                Email = admin.Email,
                Password = admin.Password,
                AdminImage = admin.AdminImage,
            };
        }

        public async Task<AdminsDTO> addAdminsAsync(CreateAdmins createAdmins)
        {
            if (createAdmins.AdminImage == null)
            {
                throw new ArgumentException("Admin image cannot be null."); 
            }

            var passwordHasher = new PasswordHasher<Admin>();
            var fileName = await SaveImageFileAsync(createAdmins.AdminImage);

            var admin = new Admin
            {
                FullName = createAdmins.FullName,
                Email = createAdmins.Email,
                Password = createAdmins.Password,
                AdminImage = fileName
            };

            admin.Password = passwordHasher.HashPassword(admin, createAdmins.Password);

            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();

            return new AdminsDTO
            {
                FullName = admin.FullName,
                Email = admin.Email,
                AdminImage = fileName,
                Password = passwordHasher.HashPassword(admin, admin.Password),
            };
        }


        public async Task<AdminsDTO> UpdeteAdminAsync(int id, CreateAdmins createAdmins)
        {
            var UpdeteAdmin = await _context.Admins.FindAsync(id);

            if (UpdeteAdmin == null)
            {

                throw new KeyNotFoundException($"Admin with ID {id} not found.");

            }

            UpdeteAdmin.FullName = createAdmins.FullName ?? UpdeteAdmin.FullName;
            UpdeteAdmin.Email = createAdmins.Email ?? UpdeteAdmin.Email;
            UpdeteAdmin.Password = createAdmins.Password ?? UpdeteAdmin.Password;

            if (createAdmins.AdminImage != null)
            {
                if (!string.IsNullOrEmpty(UpdeteAdmin.AdminImage))
                {
                    await DeleteImageFileAsync(UpdeteAdmin.AdminImage);
                }
                var fileName = await SaveImageFileAsync(createAdmins.AdminImage);
                UpdeteAdmin.AdminImage = fileName;
            }

            _context.Admins.Update(UpdeteAdmin);
            await _context.SaveChangesAsync();

            return new AdminsDTO
            {

                AdminId = UpdeteAdmin.AdminId,
                AdminImage = UpdeteAdmin.AdminImage,
                FullName = UpdeteAdmin.FullName,
                Email = UpdeteAdmin.Email,
                Password = UpdeteAdmin.Password,

            };

        }

        public async Task DeleteAdminsAsync(int id)
        {
            var DeletedAdmin = await _context.Admins.FindAsync(id);

            if (DeletedAdmin == null)
            {

                throw new Exception("This Admin Is Not Defined");

            }

            _context.Admins.Remove(DeletedAdmin);
            await _context.SaveChangesAsync();
        }



    }
}
