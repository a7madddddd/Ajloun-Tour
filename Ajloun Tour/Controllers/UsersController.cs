using Ajloun_Tour.DTOs.AdminsDTOs;
using Ajloun_Tour.DTOs.LoginDTOs;
using Ajloun_Tour.DTOs.UsersDTOs;
using Ajloun_Tour.Implementations;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersDTO>>> GetAllUsers() {
        
            var users = await _usersRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("id")]
        public async Task<ActionResult<UsersDTO>> GetUserById(int id) { 
        
            var user  = await _usersRepository.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromForm] CreateUsers createUsers)
        {


            var newUser = await _usersRepository.AddUserAsync(createUsers);
            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var response = await _usersRepository.LoginAsync(loginDTO);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred during login" });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<User>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // Implement get profile logic
            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<UsersDTO>> UpdateUserAsync(int id,[FromBody] CreateUsers createUsers) {
        
            var UpdateUser  = await _usersRepository.UpdateUsersAsync(id, createUsers);
            return Ok(UpdateUser);
        }
        [Authorize]
        [HttpDelete("id")]
        public async Task<ActionResult> DeleteUserAsync(int id) {

            await _usersRepository.DeleteUserByIdAsync(id);
            return Ok();
        }
    }
}
