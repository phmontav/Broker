using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyBrokerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyBrokerTests.Services
{
    public class StockDataServiceTests : IClassFixture<StockDataService> 
    {
        private readonly IStockDataService stockDataService;
        private readonly ILogger logger;
        private readonly IConfiguration config;

        public StockDataServiceTests()
        {
            //this.logger = logger;
            //this.config = config;
        }
        [Fact]
        public async void getStockDataValid()
        {

            string ticker = "PETR4.SA";
            Mock<IStockDataService> mock = new Mock<IStockDataService>();
            mock.Setup(m => m.getStockPrice(ticker)).Returns(Task.FromResult(decimal.Parse("20.0")));
            Assert.IsType<decimal>(await this.stockDataService.getStockPrice(ticker));
        }
        [Fact]
        public async void getStockDataInvalid() {
            string ticker = "paetr4.Sa";
            Assert.ThrowsAsync<ArgumentNullException>( async() => await stockDataService.getStockPrice(ticker));
        }
    }
}
