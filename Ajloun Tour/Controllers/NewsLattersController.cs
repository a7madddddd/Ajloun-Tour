using Ajloun_Tour.DTOs.NewsLattersDTO;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsLattersController : ControllerBase
    {
        private readonly INewsLattersRepository _newsLattersRepository;

        public NewsLattersController(INewsLattersRepository newsLattersRepository)
        {
            _newsLattersRepository = newsLattersRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsDTO>>> GetAllNewsLatters()
        {

            var allNews = await _newsLattersRepository.GetAllNewsAsync();
            return Ok(allNews);
        }

        [HttpGet("id")]
        public async Task<ActionResult<NewsDTO>> GetNewsById(int id)
        {

            var News = await _newsLattersRepository.GetNewsByIdAsync(id);
            return Ok(News);
        }

        [HttpPost]
        public async Task<ActionResult<NewsDTO>> AddNewsAsync([FromForm] CreateNews createNews)
        {

            var AddNews = await _newsLattersRepository.AddNewsAsync(createNews);
            return Ok(AddNews);
        }
        [HttpPut]
        public async Task<ActionResult<NewsDTO>> updateNewsAsync(int id, [FromBody] CreateNews createNews)
        {

            var updatedNews = await _newsLattersRepository.UpdateNewsAsync(id, createNews);
            return Ok(updatedNews);
        }
        [HttpDelete("id")]
        public async void DeleteNewsAsync(int id)
        {
            await _newsLattersRepository.DeleteNewsByIdAsync(id);
           
        }
    }
}
