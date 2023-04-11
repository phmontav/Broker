using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBrokerLibrary
{
    public class StockDataService : IStockDataService
    {
        private readonly IConfiguration config;
        private readonly ILogger<StockDataService> logger;

        public StockDataService(IConfiguration config, ILogger<StockDataService> logger )
        {
            this.config = config;
            this.logger = logger;
        }
        public async Task<decimal> getStockPrice(string ticker)
        {
            string queryUrl = $"https://brapi.dev/api/quote/{ticker}";
            Uri queryUri = new Uri(queryUrl);
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(queryUri);
                    var json = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response to get the market price
                    JObject responseJson = JObject.Parse(json);
                    string marketPrice = (string)responseJson["results"][0]["regularMarketPrice"];
                    Console.WriteLine($"The market price of {ticker} is {marketPrice}");
                    return decimal.Parse(marketPrice);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error getting stock data", ex);
                throw;
            }

        }
    }
}
