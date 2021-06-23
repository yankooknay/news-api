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
            filter = string.IsNullOrWhiteSpace(filter) ? "" : " " + filter.Trim();
            return await newsItemRepository.GetAll().Where(e => e.Title.Contains(filter)).OrderByDescending(e => e.Time).Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
        }
    }
}
