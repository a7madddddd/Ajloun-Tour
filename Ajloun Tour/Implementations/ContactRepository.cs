using Ajloun_Tour.DTOs.ContactDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class ContactRepository : IContactRepository
    {
        private readonly MyDbContext _context;

        public ContactRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ContactDTO>> GetALLContact()
        {
            var contacts = await _context.ContactMessages.ToListAsync();

            return contacts.Select(c => new ContactDTO
            {

                MessageId = c.MessageId,
                Name = c.Name,
                Email = c.Email,
                Subject = c.Subject,
                Message = c.Message,
                SubmittedAt = c.SubmittedAt,
                IsRead = c.IsRead,
            });
        }

        public async Task<ContactDTO> GetContactById(int id)
        {
            var contact = await _context.ContactMessages.FindAsync(id);

            if (contact == null)
            {

                throw new Exception("This Contact Is Not Defined");
            }

            return new ContactDTO
            {

                MessageId = contact.MessageId,
                Name = contact.Name,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message,
                SubmittedAt = contact.SubmittedAt,
                IsRead = contact.IsRead,

            };
        }

        public async Task<ContactDTO> AddContactAsync(CreateContact createContact)
        {
            var contact = new ContactMessage
            {

                Name = createContact.Name,
                Email = createContact.Email,
                Subject = createContact.Subject,
                Message = createContact.Message,
                SubmittedAt = createContact.SubmittedAt,
                IsRead = createContact.IsRead,

            };

            await _context.ContactMessages.AddAsync(contact);
            _context.SaveChanges();

            return new ContactDTO
            {

                MessageId = contact.MessageId,
                Name = contact.Name,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message,
                SubmittedAt = contact.SubmittedAt,
                IsRead = contact.IsRead,
            };
        }

        public async Task<ContactDTO> UpdateContactAsync(int id, CreateContact createContact)
        {
            var updatecontact = await _context.ContactMessages.FindAsync(id);

            if (updatecontact == null)
            {

                throw new ArgumentNullException(nameof(updatecontact));
            };

            updatecontact.Name = createContact.Name ?? updatecontact.Name;
            updatecontact.Email = createContact.Email ?? updatecontact.Email;
            updatecontact.Subject = createContact.Subject ?? updatecontact.Subject;
            updatecontact.Message = createContact.Message ?? updatecontact.Message;
            updatecontact.SubmittedAt = createContact?.SubmittedAt ?? updatecontact.SubmittedAt;
            updatecontact.IsRead = createContact?.IsRead ?? updatecontact.IsRead;


            _context.ContactMessages.Update(updatecontact);
            await _context.SaveChangesAsync();


            return new ContactDTO
            {

                MessageId = updatecontact.MessageId,
                Name = updatecontact.Name,
                Email = updatecontact.Email,
                Subject = updatecontact.Subject,
                Message = updatecontact.Message,
                SubmittedAt = updatecontact.SubmittedAt,
                IsRead = updatecontact.IsRead,
            };
        }

        public async Task DeleteContactById(int id)
        {
            var deletedContact = await _context.ContactMessages.FindAsync(id);

            if (deletedContact == null)
            {

                throw new ArgumentNullException(nameof(deletedContact));

            }

            _context.ContactMessages.Remove(deletedContact);
            await _context.SaveChangesAsync();
        }

    }
}
