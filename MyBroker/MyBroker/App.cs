using MyBrokerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBroker
{
    internal class App
    {
        private readonly IStockDataService stockDataService;

        public App(IStockDataService stockDataService)
        {
            this.stockDataService = stockDataService;
        }

        public async Task Run(string[] args)
        {
            var aux = await this.stockDataService.getStockPrice("PETR4.SA");
           Console.WriteLine(aux);
        }
    }
}
