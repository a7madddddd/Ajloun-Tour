using Ajloun_Tour.DTOs.AdminsDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {

        private readonly IAdminsRepository _adminsRepository;

        public AdminsController(IAdminsRepository adminsRepository)
        {
            _adminsRepository = adminsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminsDTO>>> GetAllAdminsAsync() {


            var Admins = await _adminsRepository.GetAdminsAsync();
            return Ok(Admins);
        }
        [HttpGet("id")]
        public async Task<ActionResult<AdminsDTO>> GetAdminsById(int id) {
        
            var admin = await _adminsRepository.GetAdminById(id);
            return Ok(admin);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdminAsync([FromForm] CreateAdmins createAdmins)
        {
            if (createAdmins.AdminImage == null)
            {
                return BadRequest("Admin image is required.");
            }

            var admin = await _adminsRepository.addAdminsAsync(createAdmins);
            return Ok(admin);
        }



        [HttpPut("id")]
        public async Task<ActionResult<AdminsDTO>> UpdeteAdminAsync(int id, [FromBody] CreateAdmins createAdmins) {

            var updateAdmin = await _adminsRepository.UpdeteAdminAsync(id, createAdmins);
            return Ok(updateAdmin);
        }
        [HttpDelete("id")]
        public async void DeleteAdminAsync(int id) {

            await _adminsRepository.DeleteAdminsAsync(id);
        } 
    }
}
