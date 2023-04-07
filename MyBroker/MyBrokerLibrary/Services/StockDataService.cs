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
            Console.WriteLine(this.config.GetSection("ConnectionString")["X_RAPIDAPI_KEY"]);
            return this.config.GetSection("ConnectionString")["X_RAPIDAPI_KEY"];
        }
        public async Task<decimal> getStockPrice(string ticker)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://mboum-finance.p.rapidapi.com/qu/quote?symbol={ticker}"),
                Headers =
                {
                    { "X-RapidAPI-Key", getApiKey() },
                    { "X-RapidAPI-Host", "mboum-finance.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                var regularMarketPrice = JArray.Parse(body)[0]["regularMarketPrice"].ToString();
                Console.WriteLine(regularMarketPrice);
                return Decimal.Parse(regularMarketPrice);
                
            }
        }
    }
}
