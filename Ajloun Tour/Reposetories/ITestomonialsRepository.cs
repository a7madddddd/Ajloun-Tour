using Ajloun_Tour.DTOs.TestoDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface ITestomonialsRepository
    {
        Task<IEnumerable<TestoDTO>> GetAllTestoAsync();

        Task<TestoDTO> GetTestoById(int id);

        Task<TestoDTO> AddTestoAsync(CreateTesto createTesto);

        Task DeleteTestoAsync(int id);
    }
}
