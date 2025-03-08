using Ajloun_Tour.DTOs.LoginDTOs;
using Ajloun_Tour.DTOs.ToursDTOs;
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
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersRepository(MyDbContext context, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UsersImages");
            EnsureImageDirectoryExists();
            _configuration = configuration;
            _passwordHasher = passwordHasher;
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
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile), "Image file is required.");
            }

            string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            string filePath = Path.Combine(_imageDirectory, fileName);

            // Ensure the directory exists
            Directory.CreateDirectory(_imageDirectory);

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
                new Claim(ClaimTypes.MobilePhone, user.Phone),
                new Claim(ClaimTypes.Name, user.FullName),
            }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:ExpiryInHours"])),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
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
                UserImage = u.UserImage,
                CreatedAt = DateTime.UtcNow,
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
                CreatedAt = user.CreatedAt,
                Password = user.Password,
                UserImage = user.UserImage,
                Phone = user.Phone,

            };
        }

        public async Task<UsersDTO> AddUserAsync(CreateUsers createUsers)
        {

            var passwordHasher = new PasswordHasher<User>();
            var fileName = await SaveImageFileAsync(createUsers.UserImage);


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
                Password = user.Password

            };
        }

        public async Task<UsersDTO> UpdateUsersAsync(int id, CreateUsers createUsers)
        {

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            if (createUsers.UserImage != null)
            {
                // If there was an existing image, delete it before uploading the new one
                if (!string.IsNullOrEmpty(user.UserImage))
                {
                    await DeleteImageFileAsync(user.UserImage);
                }

                // Save new image and update the TourImage path
                var fileName = await SaveImageFileAsync(createUsers.UserImage);
                user.UserImage = fileName;
            }

            user.FullName = createUsers.FullName ?? user.FullName;
            user.Phone = createUsers.Phone ?? user.Phone;
            user.Email = createUsers.Email ?? user.Email;
            user.CreatedAt = createUsers.CreatedAt ?? user.CreatedAt;
            user.Password = createUsers.Password ?? user.Password;



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


        //login
        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                loginDTO.Password
            );

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {                                                                                                                                           
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                UserImage = user.UserImage,
                Token = token
            };
        }
    }
}


