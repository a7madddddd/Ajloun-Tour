using Ajloun_Tour.DTOs.ContactDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;

        public ContactsController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContactsAsync()
        {

            var contacts = await _contactRepository.GetALLContact();

            return Ok(contacts);

        }

        [HttpGet("id")]
        public async Task<ActionResult<ContactDTO>> GetContactById(int id)
        {

            var contact = await _contactRepository.GetContactById(id);
            return Ok(contact);
        }

        [HttpPost]
        public async Task<ActionResult<ContactDTO>> AddContactAsync([FromForm] CreateContact createContact)
        {

            var addContact = await _contactRepository.AddContactAsync(createContact);
            return Ok(addContact);
        }

        [HttpPut("id")]
        public async Task<ActionResult<ContactDTO>> UpdateContactAsync(int id, [FromBody] CreateContact createContact)
        {

            var updateContact = await _contactRepository.UpdateContactAsync(id, createContact);
            return Ok(updateContact);
        }

        [HttpDelete("id")]
        public async void DeleteContactById(int id)
        {

            await _contactRepository.DeleteContactById(id);

        }
    }
}
