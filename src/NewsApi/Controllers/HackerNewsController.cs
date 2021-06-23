using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using YNewsApi.Entities.Entity;
using YNewsApi.Entities.Repository;

namespace NewsApi.Controllers
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
        public IEnumerable<NewsItem> Get(string filter, int pageSize = 10, int page = 1)
        {
            filter = string.IsNullOrWhiteSpace(filter) ? "" : " " + filter.Trim();
            return newsItemRepository.GetAll().Where(e => e.Title.Contains(filter)).OrderByDescending(e => e.Time).Skip(pageSize * (page - 1)).Take(pageSize);
        }
    }
}
