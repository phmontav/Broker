using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyBrokerLibrary;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit; // Você pode usar o framework xUnit para escrever os testes


namespace MyBrokerTests.Services
{
    public class StockDataServiceTests
    {
        private IConfiguration _config;
        private ILogger<StockDataService> _logger;

        public StockDataServiceTests()
        {
            // Configurar objetos falsos (mock objects) para IConfiguration e ILogger
            _logger = new Mock<ILogger<StockDataService>>().Object;
        }

        [Fact]
        public async Task GetStockPrice_ValidTicker_ReturnsStockPrice()
        {
            // Arrange
            var stockDataService = new StockDataService( _logger);
            var ticker = "PETR4";
            var httpClient = new HttpClient();
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("{\"results\":[{\"regularMarketPrice\":\"100.00\"}]}")
            };
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var httpClientFactory = httpClientFactoryMock.Object;

            // Act
            var result = await stockDataService.getStockPrice(ticker);

            // Assert
            Assert.IsType<decimal>(result);
        }

        [Fact]
        public async Task GetStockPrice_InvalidTicker_ThrowsArgumentException()
        {
            // Arrange
            var stockDataService = new StockDataService(_logger);
            var ticker = "INVALID";
            var httpClient = new HttpClient();
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("{\"error\":\"Invalid ticker\"}")
            };
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var httpClientFactory = httpClientFactoryMock.Object;

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                var result = await stockDataService.getStockPrice(ticker);
            });
        }
    }
}
