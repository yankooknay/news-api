using System.Linq;
using System.Threading.Tasks;
using YNewsApi.Entities.Entity;

namespace YNewsApi.Entities.Repository
{
    public interface INewsItemRepository
    {
        IQueryable<NewsItem> GetAll();

        Task AddAsync(NewsItem newsItem);

        Task UpdateAsync(NewsItem newsItem);
    }
}
