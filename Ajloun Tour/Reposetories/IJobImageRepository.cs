using Ajloun_Tour.DTOs2.JobDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IJobImageRepository
    {
        Task<IEnumerable<JobImageDTO>> GetImagesByJobId(int jobId);
        Task<JobImageDTO> AddImage(int jobId, IFormFile image);
        Task DeleteImage(int imageId);
        Task<bool> DeleteImagesByJobId(int jobId);
    }
}
