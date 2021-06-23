using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsReaderWorker.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using YNews.NewsReader.Services.Interfaces;

namespace NewsReaderWorker
{
    public class Worker : BackgroundService
    {
        readonly ILogger<Worker> _logger;
        readonly AppSettings appSettings;
        readonly IServiceProvider services;

        public Worker(IServiceProvider services, ILogger<Worker> logger, IOptions<AppSettings> appSettingsOtions)
        {
            _logger = logger;
            appSettings = appSettingsOtions.Value;
            this.services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    using (IServiceScope serviceScope = services.CreateScope())
                    {
                        INewsReaderService newsReaderService = serviceScope.ServiceProvider.GetRequiredService<INewsReaderService>();
                        await newsReaderService.Run(appSettings.HackerNewsUrl, stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }

                await Task.Delay(TimeSpan.FromMinutes(appSettings.ReadNewsIntervalInMinutes), stoppingToken);
            }
        }
    }
}
