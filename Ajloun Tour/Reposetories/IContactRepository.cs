using Ajloun_Tour.DTOs.ContactDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IContactRepository
    {
        Task<IEnumerable<ContactDTO>> GetALLContact();

        Task<ContactDTO> GetContactById(int id);

        Task<ContactDTO> AddContactAsync(CreateContact createContact);

        Task<ContactDTO> UpdateContactAsync(int id, CreateContact createContact);

        Task DeleteContactById(int id);
    }
}
