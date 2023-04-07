namespace MyBrokerLibrary
{
    public interface IStockDataService
    {
        Task<decimal> getStockPrice(string ticker);
    }
}