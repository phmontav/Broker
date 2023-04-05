using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBroker
{
    internal class StockDataHandler
    {
        public async Task<decimal> getStockPrice(string ticker)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://mboum-finance.p.rapidapi.com/qu/quote?symbol={ticker}"),
                Headers =
                {
                    { "X-RapidAPI-Key", "d99c4c577amsha80bae248f2815ep1d87edjsn9716373a2ee5" },
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
