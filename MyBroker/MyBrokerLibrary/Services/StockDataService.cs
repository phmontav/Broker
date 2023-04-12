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
        private readonly ILogger<StockDataService> logger;

        public StockDataService( ILogger<StockDataService> logger )
        {
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
                    JObject responseJson = JObject.Parse(json);
                    if (responseJson.ContainsKey("error"))
                    {
                        throw new ArgumentException("Could Not find stock");
                    }
                    string marketPrice = (string)responseJson["results"][0]["regularMarketPrice"];
                    this.logger.LogInformation($"The market price of {ticker} is {marketPrice}");
                    return decimal.Parse(marketPrice);
                }
            }
            catch (Exception ex)
            {
                if(ex is ArgumentException)
                {
                    this.logger.LogCritical(ex.Message);
                    throw;
                }
                if (ex is HttpRequestException)
                {
                    throw;
                }
                
            }
            return -1;

        }
    }
}
