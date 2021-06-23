using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsReaderWorker.Models;
using YNews.DataAccess.Db;
using YNews.DataAccess.Repository;
using YNews.NewsReader.Services;
using YNews.NewsReader.Services.Interfaces;
using YNewsApi.Entities.Repository;

namespace NewsReaderWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<NewsCacheContext>(o => o.UseSqlServer(hostContext.Configuration.GetConnectionString("NewsCache")));
                    services.AddTransient<INewsReaderService, NewsReaderService>();
                    services.AddTransient<INewsItemRepository, NewsItemRepository>();
                    services.AddHttpClient();
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
                    services.AddHostedService<Worker>();
                })
                .UseWindowsService();
    }
}
