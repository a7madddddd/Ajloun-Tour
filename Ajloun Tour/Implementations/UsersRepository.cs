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
    public class UsersRepository : IUsersRepository
    {
        private readonly MyDbContext _context;
        private readonly string _imageDirectory;
        private readonly IConfiguration _configuration;
        public UsersRepository(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UsersImages");
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return null; // User not found
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result != PasswordVerificationResult.Success)
            {
                return null; // Invalid password
            }

            // Generate JWT Token
            return GenerateJwtToken(user);
        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.FullName)
        }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:ExpiryInHours"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IEnumerable<UsersDTO>> GetAllUsersAsync()
        {
            var Users = await _context.Users.ToListAsync();

            return Users.Select(u => new UsersDTO
            {

                UserId = u.UserId,
                Email = u.Email,
                FullName = u.FullName,
                Password = u.Password,
                Phone = u.Phone,
                CreatedAt = DateTime.UtcNow,
                UserImage = u.UserImage,
            });
        }

        public async Task<UsersDTO> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {

                throw new Exception("This User With This Id Is Not Defined");
            }
            return new UsersDTO
            {

                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName,
                UserImage = user.UserImage,
                CreatedAt = user.CreatedAt,
                Password = user.Password,
                Phone = user.Phone,

            };
        }

        public async Task<UsersDTO> AddUserAsync( CreateUsers createUsers)
        {
            if (createUsers.ImageFile == null)
            {
                throw new ArgumentNullException(nameof(createUsers.ImageFile), "Image file is required.");
            }

            var passwordHasher = new PasswordHasher<User>();
            var fileName = await SaveImageFileAsync(createUsers.ImageFile);

            var user = new User
            {

                FullName = createUsers.FullName,
                Email = createUsers.Email,
                Phone = createUsers.Phone,
                CreatedAt = createUsers.CreatedAt,
                UserImage = fileName

            };

            user.Password = passwordHasher.HashPassword(user, createUsers.Password);


            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UsersDTO
            {

                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                Phone = user.Phone,
                Email = user.Email,
                UserImage = fileName,
                Password = passwordHasher.HashPassword(user, user.Password),

            };
        }

        public async Task<UsersDTO> UpdateUsersAsync(int id,  CreateUsers createUsers)
        {

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            user.FullName = createUsers.FullName ?? user.FullName;
            user.Phone = createUsers.Phone ?? user.Phone;
            user.Email = createUsers.Email ?? user.Email;
            user.CreatedAt = createUsers.CreatedAt ?? user.CreatedAt;
            user.Password = createUsers.Password ?? user.Password;


            if (createUsers.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(user.UserImage))
                {
                    await DeleteImageFileAsync(user.UserImage);
                }
                var fileName = await SaveImageFileAsync(createUsers.ImageFile);
                user.UserImage = fileName;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UsersDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Password = user.Password,
                CreatedAt = user.CreatedAt,
                UserImage = user.UserImage,
               
            };
        }

        public async Task DeleteUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
