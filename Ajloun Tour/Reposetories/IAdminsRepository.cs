using Ajloun_Tour.DTOs.AdminsDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IAdminsRepository
    {
        Task <IEnumerable<AdminsDTO>>GetAdminsAsync ();

        Task<AdminsDTO> GetAdminById(int id);

        Task <AdminsDTO> addAdminsAsync ( CreateAdmins createAdmins);

        Task<AdminsDTO> UpdeteAdminAsync (int id, CreateAdmins createAdmins);

        Task DeleteAdminsAsync (int id);
    }
}
