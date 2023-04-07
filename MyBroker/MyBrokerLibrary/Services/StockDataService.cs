using Microsoft.Extensions.Configuration;
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

        public StockDataService(IConfiguration config )
        {
            this.config = config;
        }

        private string getApiKey()
        {
            Console.WriteLine(this.config.GetSection("ConnectionString")["AlphaVantageKey"]);
            return this.config.GetSection("ConnectionString")["AlphaVantageKey"];
        }
        public async Task<decimal> getStockPrice(string ticker)
        {
            string apiKey = getApiKey();
            string queryUrl = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={ticker}&apikey={apiKey}";
            Uri queryUri = new Uri( queryUrl );
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(queryUri);
                    var json = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response to get the market price
                    JObject responseJson = JObject.Parse(json);
                    string marketPrice = (string)responseJson["Global Quote"]["05. price"];
                    Console.WriteLine($"The market price of {ticker} is {marketPrice}");
                    return decimal.Parse(marketPrice);
                }
            }
            catch (Exception)
            {

                throw;
            }
            

        }
    }
}
