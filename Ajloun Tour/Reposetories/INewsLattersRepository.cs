using Ajloun_Tour.DTOs.NewsLattersDTO;
using Ajloun_Tour.Models;

namespace Ajloun_Tour.Reposetories
{
    public interface INewsLattersRepository
    {
        Task<IEnumerable<NewsDTO>> GetAllNewsAsync();

        Task <NewsDTO> GetNewsByIdAsync(int id);

        Task<NewsDTO> AddNewsAsync(CreateNews createNews);

        Task<NewsDTO> UpdateNewsAsync(int id, CreateNews createNews);

        Task DeleteNewsByIdAsync(int id);
    }


}
