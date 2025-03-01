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
                IsActive = n.IsActive,
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
                IsActive=news.IsActive,
            };
        }

        public async Task<NewsDTO> AddNewsAsync(CreateNews createNews)
        {
            

            var news = new NewsletterSubscriber
            {
                Email = createNews.Email,
                SubscribedAt = createNews.SubscribedAt = DateTime.Now,
            };

            _context.NewsletterSubscribers.Add(news);
            await _context.SaveChangesAsync();

            return new NewsDTO
            {
                Email = news.Email,
                SubscribedAt = news.SubscribedAt,
                IsActive = news.IsActive,

            };
        }

        public async Task<NewsDTO> UpdateNewsAsync(int id, CreateNews createNews)
        {
            var updatedNews = await _context.NewsletterSubscribers.FindAsync(id);

            if (updatedNews == null) {

                throw new Exception("This News Is Not Defined");
            }

            updatedNews.Email = createNews.Email ?? updatedNews.Email;
            updatedNews.SubscribedAt = createNews?.SubscribedAt ?? DateTime.Now;
            updatedNews.IsActive = createNews?.IsActive ?? updatedNews.IsActive;



            _context.NewsletterSubscribers.Update(updatedNews);
            await _context.SaveChangesAsync();

            return new NewsDTO { 
            
                Email = updatedNews.Email,
                SubscribedAt = updatedNews.SubscribedAt,
                SubscriberId = updatedNews.SubscriberId,
                IsActive = updatedNews.IsActive,
            };
        }

        public async Task DeleteNewsByIdAsync(int id)
        {
            var Deletenews = await _context.NewsletterSubscribers.FindAsync(id);

            if (Deletenews == null) {

                throw new Exception("This News Is Not Defined");
            }

            _context.NewsletterSubscribers.Remove(Deletenews);
            await _context.SaveChangesAsync();
        }

    }
}
