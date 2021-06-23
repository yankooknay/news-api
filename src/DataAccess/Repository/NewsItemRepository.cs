using YNews.DataAccess.Db;
using YNewsApi.Entities.Entity;
using YNewsApi.Entities.Repository;

namespace YNews.DataAccess.Repository
{
    public class NewsItemRepository : GenericRepository<NewsItem>, INewsItemRepository
    {
        public NewsItemRepository(NewsCacheContext context) : base(context) { }
    }
}
