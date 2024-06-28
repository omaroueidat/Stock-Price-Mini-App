namespace StocksApp.ServiceContract
{
    public interface IFinnhubService
    {
        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}
