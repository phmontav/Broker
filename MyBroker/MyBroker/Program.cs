using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBroker;
using MyBrokerLibrary;

using IHost host = CreateHostBuilder(args).Build();
IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    await services.GetRequiredService<App>().Run(args);
}
    
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IStockDataService, StockDataService>();
            services.AddSingleton<App>();
        });
}
