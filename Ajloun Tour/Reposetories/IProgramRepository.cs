using Ajloun_Tour.DTOs2.ProgramDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IProgramRepository
    {
        Task<IEnumerable<ProgramDTO>> GetAllPrograms();
        Task<ProgramDTO> GetProgramById(int id);
        Task <ProgramDTO> AddProgramAsync(CreateProgram createProgram);
        Task <ProgramDTO> UpdateProgramAsync(int id, CreateProgram createProgram);
        Task DeleteProgramById(int id);
    }
}
