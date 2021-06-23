using System.Threading;
using System.Threading.Tasks;

namespace YNews.NewsReader.Services.Interfaces
{
    public interface INewsReaderService
    {
        Task Run(string baseUrl, CancellationToken cancellationToken);
    }
}
