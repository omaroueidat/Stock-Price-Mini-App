using StocksApp.ServiceContract;
using System.Text.Json;

namespace StocksApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public FinnhubService(IHttpClientFactory httpClienFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClienFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    //
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["api-key"]}"),
                    Method = HttpMethod.Get,

                };

                // Send the request to the server in the url and Recieve the response
                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponse.Content.ReadAsStream();

                StreamReader sr = new StreamReader(stream);

                string response = sr.ReadToEnd();

                Dictionary<string, object>? responseDict =  JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (responseDict is null)
                {
                    throw new InvalidOperationException();
                }

                if (responseDict.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(responseDict["error"]));
                }
                return responseDict;
            }
        }
    }
}
