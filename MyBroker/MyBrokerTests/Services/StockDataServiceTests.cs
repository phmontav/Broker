using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyBrokerLibrary;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;


namespace MyBrokerTests.Services
{
    public class StockDataServiceTests
    {
        private IConfiguration _config;
        private ILogger<StockDataService> _logger;

        public StockDataServiceTests()
        {
            _logger = new Mock<ILogger<StockDataService>>().Object;
        }

        [Fact]
        public async Task GetStockPrice_ValidTicker_ReturnsStockPrice()
        {
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

            var result = await stockDataService.getStockPrice(ticker);

            Assert.IsType<decimal>(result);
        }

        [Fact]
        public async Task GetStockPrice_InvalidTicker_ThrowsArgumentException()
        {
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

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var result = await stockDataService.getStockPrice(ticker);
            });
        }
    }
}
