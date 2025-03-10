using Ajloun_Tour.DTOs2.JobApplicationDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<JobApplicationDTO>> GetAllApplications();
        Task<JobApplicationDTO> GetApplicationById(int id);
        Task<IEnumerable<JobApplicationDTO>> GetApplicationsByJobId(int jobId);
        Task<IEnumerable<JobApplicationDTO>> GetApplicationsByEmail(string email);
        Task<JobApplicationDTO> AddApplication(CreateApplication createApplication);
        Task<JobApplicationDTO> UpdateApplicationStatus(int id, string status);
        Task DeleteApplication(int id);
    }
}
