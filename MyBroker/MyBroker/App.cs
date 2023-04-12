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
                if(args.Length != 3)
                {
                    throw new ArgumentException("Wrong number of arguments passed.It must be 3 arguments:Stock SellPrice BuyPrice");

                }
                ticker = args[0];
                ticker = ticker.ToUpper();
                sellPrice = decimal.Parse(args[1]);
                buyPrice = decimal.Parse(args[2]);
                while (true)
                {
                    try
                    {
                        var regularMarketPrice = await this.stockDataService.getStockPrice(ticker);
                        if (regularMarketPrice >= sellPrice)
                        {
                            await this.emailService.sendEmail(ticker, "sell", regularMarketPrice.ToString());
                        }
                        if (regularMarketPrice <= buyPrice)
                        {
                            await this.emailService.sendEmail(ticker, "buy", regularMarketPrice.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is ArgumentException) throw;
                        if(ex is HttpRequestException)
                        {
                            this.logger.LogCritical("Could not connect to server");
                        }
                    }
                    await Task.Delay(TimeSpan.FromSeconds(12));
                }
            }
            catch(Exception ex) {
                if(ex is ArgumentException)
                {
                    this.logger.LogCritical("Error with the command line arguments",ex);
                    throw;
                }
                if(ex is FormatException)
                {
                    this.logger.LogCritical("Error with the command line arguments, The prices must consist of only numbers", ex);
                }
            }
            
        }
    }
}
