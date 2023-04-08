using Microsoft.Extensions.Logging;
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
        private readonly IEmailService emailService;
        private readonly ILogger<App> logger;

        public App(IStockDataService stockDataService, IEmailService emailService,ILogger<App> logger)
        {
            this.stockDataService = stockDataService;
            this.emailService = emailService;
            this.logger = logger;
        }

        public async Task Run(string[] args)
        {
            string ticker = "";
            decimal buyPrice = 0, sellPrice = 0;
            try
            {
                ticker = args[0];
                ticker = ticker.ToUpper();
                buyPrice = decimal.Parse(args[1]);
                sellPrice = decimal.Parse(args[2]);
            }
            catch(Exception ex) {
                this.logger.LogCritical("Error with the command line arguments",ex);
                throw;
            }
            while (true)
            {
                try
                {   
                    var regularMarketPrice = await this.stockDataService.getStockPrice(ticker + ".SA");
                    if (regularMarketPrice >= sellPrice)
                    {
                        await this.emailService.sendEmail(ticker, "sell", regularMarketPrice.ToString());
                    }
                    if (regularMarketPrice <= buyPrice) {
                        await this.emailService.sendEmail(ticker, "buy", regularMarketPrice.ToString());
                    }
                }
                catch (Exception)
                {
                    
                }
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}
