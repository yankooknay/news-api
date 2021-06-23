using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using YNews.NewsReader.Services.Interfaces;
using YNewsApi.Entities.Entity;
using YNewsApi.Entities.Repository;

namespace YNews.NewsReader.Services
{
    // TODO: enhance the process to update only items that changed using 'updates' endpoint and remove the dead ones.

    public class NewsReaderService : INewsReaderService
    {
        readonly HttpClient httpClient;
        readonly INewsItemRepository newsItemRepository;

        public NewsReaderService(IHttpClientFactory httpClientFactory, INewsItemRepository newsItemRepository)
        {
            this.httpClient = httpClientFactory.CreateClient();
            this.newsItemRepository = newsItemRepository;
        }

        /// <summary>
        /// Read Hacker News stories and save them to the database.
        /// </summary>
        public async Task Run(string baseUrl, CancellationToken cancellationToken)
        {
            HashSet<int> idsFromDb = newsItemRepository.GetAll().Select(e => e.Id).ToHashSet();

            string url = $"{baseUrl}newstories.json";
            HttpResponseMessage httpResponseMessage = await this.httpClient.GetAsync(url, cancellationToken);

            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                string stringResponse = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
                IEnumerable<int> idsFromFeed = JArray.Parse(stringResponse).Select(x => (int)x);

                foreach (int id in idsFromFeed)
                {
                    await ReadAndCacheNewsItem(baseUrl, id, idsFromDb, cancellationToken);
                }
            }
            else
            {
                throw new Exception("Something went wrong reading Hacker News API newstories list");
            }
        }

        async Task ReadAndCacheNewsItem(string baseUrl, int id, HashSet<int> idsFromDb, CancellationToken cancellationToken)
        {
            string url = $"{baseUrl}item/{id}.json";
            HttpResponseMessage httpResponseMessage = await this.httpClient.GetAsync(url, cancellationToken);

            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                string stringResponse = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
                NewsItem newsItem = JsonConvert.DeserializeObject<NewsItem>(stringResponse);

                if (newsItem.Type == "story" && !newsItem.Dead)
                {
                    if (idsFromDb.Contains(id))
                    {
                        await newsItemRepository.UpdateAsync(newsItem);
                    }
                    else
                    {
                        await newsItemRepository.AddAsync(newsItem);
                    }
                }
            }
            else
            {
                throw new Exception("Something went wrong reading Hacker News API item");
            }
        }
    }
}
