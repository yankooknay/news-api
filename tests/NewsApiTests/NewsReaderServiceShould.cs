using YNews.NewsApi.Controllers;
using System;
using Xunit;
using YNewsApi.Entities.Repository;
using Moq;
using System.Collections.Generic;
using YNewsApi.Entities.Entity;
using System.Linq;
using MockQueryable.Moq;
using YNews.NewsReader.Services;
using System.Net.Http;
using System.Threading.Tasks;
using Moq.Protected;
using System.Threading;
using System.Net;
using Newtonsoft.Json;

namespace NewsApiTests
{
    // TODO:  complete all tests for all paths in NewsReaderService logic

    public class NewsReaderServiceShould
    {
        readonly NewsReaderService newsReaderService;
        readonly Mock<INewsItemRepository> mockNewsItemRepository;
        readonly Mock<IHttpClientFactory> mockHttpClientFactory;
        readonly Mock<HttpMessageHandler> mockHttpMessageHandler;

        public NewsReaderServiceShould()
        {
            mockNewsItemRepository = new Mock<INewsItemRepository>();
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(s => s.CreateClient(It.IsAny<string>())).Returns(new HttpClient(mockHttpMessageHandler.Object));

            newsReaderService = new NewsReaderService(mockHttpClientFactory.Object, mockNewsItemRepository.Object);
        }

        [Fact]
        [Trait("Categories", "NewsApiImplementation")]
        public async void Run()
        {
            // Arrange
            List<NewsItem> newsItems = new();
            mockNewsItemRepository.Setup(s => s.GetAll()).Returns(newsItems.AsQueryable().BuildMock().Object);

            HttpResponseMessage ghostInspectorHttpResponseMessage = new(HttpStatusCode.Unauthorized);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(o => o.RequestUri.AbsolutePath == "/newstories.json"), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(ghostInspectorHttpResponseMessage);

            // Act
            Task response = newsReaderService.Run("http://localhost/", CancellationToken.None);

            // Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal("Something went wrong reading Hacker News API newstories list", exception.Message);
            mockNewsItemRepository.Verify(x => x.GetAll(), Times.Once());
        }

     
    }
}
