using Ajloun_Tour.DTOs2.JobDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IJobRepository
    {
        Task<IEnumerable<JobDTO>> GetAllJobs(bool includeInactive = false);
        Task<JobDTO> GetJobById(int id);
        Task<JobDTO> AddJob(CreateJobDTO createJob);
        Task<JobDTO> UpdateJob(int id, UpdateJobDTO updateJob);
        Task DeleteJob(int id);
        Task<IEnumerable<JobDTO>> GetJobsByType(string jobType);
    }
}
