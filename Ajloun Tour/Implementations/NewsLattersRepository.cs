using Ajloun_Tour.DTOs.NewsLattersDTO;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class NewsLattersRepository : INewsLattersRepository
    {
        private readonly MyDbContext _context;

        public NewsLattersRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NewsDTO>> GetAllNewsAsync()
        {
            var news = await _context.NewsletterSubscribers.ToListAsync();

            return news.Select(n => new NewsDTO
            {
                SubscriberId = n.SubscriberId,
                Email = n.Email,
                SubscribedAt = n.SubscribedAt,
            });
        }

        public async Task<NewsDTO> GetNewsByIdAsync(int id)
        {
            var news = await _context.NewsletterSubscribers.FindAsync(id);

            if (news == null)
            {

                throw new Exception("This News Is Not Defined");
            }

            return new NewsDTO
            {

                SubscriberId = news.SubscriberId,
                Email = news.Email,
                SubscribedAt = news.SubscribedAt,
            };
        }

        public async Task<NewsDTO> AddNewsAsync(CreateNews createNews)
        {
            

            var news = new NewsletterSubscriber
            {
                Email = createNews.Email,
                SubscribedAt = createNews.SubscribedAt,
            };

            _context.NewsletterSubscribers.Add(news);
            await _context.SaveChangesAsync();

            return new NewsDTO
            {
                Email = news.Email,
                SubscribedAt = news.SubscribedAt,

            };
        }

        public Task<NewsDTO> UpdateNewsAsync(int id, CreateNews createNews)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNewsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
