using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YNews.NewsReader.Services.Interfaces;

namespace NewsReaderWorker
{
    public class ProcessRunner
    {
        readonly IServiceProvider services;

        public ProcessRunner(IServiceProvider services)
        {
            this.services = services;
        }

        async Task Run()
        {
            using IServiceScope serviceScope = services.CreateScope();
            INewsReaderService processWorker = serviceScope.ServiceProvider.GetRequiredService<INewsReaderService>();
        }
    }
}
