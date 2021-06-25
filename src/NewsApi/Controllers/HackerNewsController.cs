using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            return await newsItemRepository.GetAll()
                .Where(BuildFilter(filter))
                .OrderByDescending(e => e.Time)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        [HttpGet]
        [Route("total")]
        public async Task<int> Total(string filter)
        {
            return await newsItemRepository.GetAll().Where(BuildFilter(filter)).CountAsync();
        }

        Expression<Func<NewsItem, bool>> BuildFilter(string filter)
        {
            filter = filter == null ? "" : filter.Trim();
            string filterMiddleSentence = filter == "" ? "" : " " + filter;
            return e => e.Title.StartsWith(filter) || e.Title.Contains(filterMiddleSentence);
        }
    }
}
