using Ajloun_Tour.DTOs2.EmployeeDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync();
        Task<EmployeeDTO> GetEmployeeById(int id);
        Task<EmployeeDTO> AddEmployeeAsync(CreateEmployee createEmployee);
        Task<EmployeeDTO> UpdateEmployeeAsync(int id, CreateEmployee createEmployee);
        Task DeleteEmployeeAsync(int id);
    }
}
