using Ajloun_Tour.DTOs.LoginDTOs;
using Ajloun_Tour.DTOs.UsersDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<UsersDTO>> GetAllUsersAsync();

        Task<UsersDTO> GetUserByIdAsync(int id);

        Task<UsersDTO> AddUserAsync(CreateUsers createUsers);

        Task<UsersDTO> UpdateUsersAsync(int id, CreateUsers createUsers);

        Task DeleteUserByIdAsync(int id);


        //login
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO);


    }
}
