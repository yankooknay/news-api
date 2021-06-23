using YNews.NewsApi.Controllers;
using System;
using Xunit;
using YNewsApi.Entities.Repository;
using Moq;
using System.Collections.Generic;
using YNewsApi.Entities.Entity;
using System.Linq;
using MockQueryable.Moq;

namespace NewsApiTests
{
    public class HackerNewsControllerShould
    {
        readonly HackerNewsController hackerNewsController;
        readonly Mock<INewsItemRepository> mockNewsItemRepository;

        public HackerNewsControllerShould()
        {
            mockNewsItemRepository = new Mock<INewsItemRepository>();

            hackerNewsController = new HackerNewsController(mockNewsItemRepository.Object);
        }

        [Fact]
        [Trait("Categories", "NewsApiImplementation")]
        public async void GetAllWithoutFilter()
        {
            // Arrange
            List<NewsItem> newsItems = new() { new NewsItem() { Id = 988, Title = "A New Story" } };
            mockNewsItemRepository.Setup(s => s.GetAll()).Returns(newsItems.AsQueryable().BuildMock().Object);

            // Act
            List<NewsItem> result = await hackerNewsController.Get(null);

            // Assert
            Assert.Single(result);
            mockNewsItemRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [Fact]
        [Trait("Categories", "NewsApiImplementation")]
        public async void GetAllWithFilter()
        {
            // Arrange
            List<NewsItem> newsItems = new() { new NewsItem() { Id = 988, Title = "A New Story" }, new NewsItem() { Id = 987, Title = "Second Story" } };

            mockNewsItemRepository.Setup(s => s.GetAll()).Returns(newsItems.AsQueryable().BuildMock().Object);

            // Act
            List<NewsItem> result = await hackerNewsController.Get("New");

            // Assert
            Assert.Single(result);
            mockNewsItemRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [Fact]
        [Trait("Categories", "NewsApiImplementation")]
        public async void GetAllWithNoMatchFilter()
        {
            // Arrange
            List<NewsItem> newsItems = new() { new NewsItem() { Id = 988, Title = "A New Story" }, new NewsItem() { Id = 987, Title = "Second Story" } };

            mockNewsItemRepository.Setup(s => s.GetAll()).Returns(newsItems.AsQueryable().BuildMock().Object);

            // Act
            List<NewsItem> result = await hackerNewsController.Get("ory");

            // Assert
            Assert.Empty(result);
            mockNewsItemRepository.Verify(x => x.GetAll(), Times.Once());
        }
    }
}
