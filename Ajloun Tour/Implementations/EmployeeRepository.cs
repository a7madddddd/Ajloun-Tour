using Ajloun_Tour.DTOs2.EmployeeDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;

namespace Ajloun_Tour.Implementations
{
    // EmployeeRepository.cs
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(
            MyDbContext context,
            IMapper mapper,
            ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Job)
                .Include(e => e.Application)
                .Select(e => new EmployeeDTO
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Email = e.Email,
                    Phone = e.Phone,
                    HireDate = e.HireDate.GetValueOrDefault(),
                    Salary = e.Salary,
                    Status = e.Status,
                    JobTitle = e.Job.Title, // Assuming Job has a 'Title' property
                    ApplicantName = e.Application != null ? e.Application.ApplicantName : null
                })
                .ToListAsync();
        }


        public async Task<EmployeeDTO> GetEmployeeById(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Application)
                .Include(e => e.Job)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                throw new NotFoundException($"Employee with ID {id} not found");

            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> AddEmployeeAsync(CreateEmployee createEmployee)
        {
            // Verify that application exists
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.ApplicationId == createEmployee.ApplicationId);

            if (application == null)
                throw new NotFoundException($"Application with ID {createEmployee.ApplicationId} not found");

            // Verify that job exists
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.JobId == createEmployee.JobId);

            if (job == null)
                throw new NotFoundException($"Job with ID {createEmployee.JobId} not found");

            var employee = _mapper.Map<Employee>(createEmployee);
            employee.HireDate = DateTime.UtcNow;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return await GetEmployeeById(employee.EmployeeId);
        }

        public async Task<EmployeeDTO> UpdateEmployeeAsync(int id, CreateEmployee createEmployee)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                throw new NotFoundException($"Employee with ID {id} not found");

            // Verify that application exists
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.ApplicationId == createEmployee.ApplicationId);

            if (application == null)
                throw new NotFoundException($"Application with ID {createEmployee.ApplicationId} not found");

            // Verify that job exists
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.JobId == createEmployee.JobId);

            if (job == null)
                throw new NotFoundException($"Job with ID {createEmployee.JobId} not found");

            _mapper.Map(createEmployee, employee);
            await _context.SaveChangesAsync();

            return await GetEmployeeById(id);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                throw new NotFoundException($"Employee with ID {id} not found");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
