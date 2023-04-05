using MyBroker;




class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        StockDataHandler handler = new StockDataHandler();
        await handler.getStockPrice("PETR4.SA");
    }
}
