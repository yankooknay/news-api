using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YNewsApi.Entities.Entity;
using YNewsApi.Entities.Repository;

namespace YNews.NewsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        readonly INewsItemRepository newsItemRepository;

        public HackerNewsController(INewsItemRepository newsItemRepository)
        {
            this.newsItemRepository = newsItemRepository;
        }

        [HttpGet]
        public async Task<List<NewsItem>> Get(string filter, int pageSize = 10, int page = 1)
        {
            string filterMiddleSentence = string.IsNullOrWhiteSpace(filter) ? "" : " " + filter.Trim();
            return await newsItemRepository.GetAll()
                .Where(e => e.Title.Contains(filter) || e.Title.Contains(filterMiddleSentence))
                .OrderByDescending(e => e.Time)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        [HttpGet]
        [Route("total")]
        public async Task<int> Total(string filter)
        {
            string filterMiddleSentence = string.IsNullOrWhiteSpace(filter) ? "" : " " + filter.Trim();
            return await newsItemRepository.GetAll().Where(e => e.Title.Contains(filter) || e.Title.Contains(filterMiddleSentence)).CountAsync();
        }
    }
}
