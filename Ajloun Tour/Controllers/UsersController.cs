using Ajloun_Tour.DTOs.UsersDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<UsersDTO>> AddUsersAsync([FromForm]CreateUsers createUsers) {
        
            var AddUser = await _usersRepository.AddUserAsync(createUsers);
            return Ok(AddUser);
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
