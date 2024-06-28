using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.ServiceContract;
using System.Data;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFinnhubService _finnhubService;

        private readonly IOptions<StockOptions> _stockOptions;

        public HomeController(IFinnhubService finnhubService, IOptions<StockOptions> stockOptions)
        {
            _finnhubService = finnhubService;
            _stockOptions = stockOptions;
        }

        [Route("/")]
        [Route("home")]
        public async Task<IActionResult> Index()
        {
            Dictionary<string, object>? response;
            Stock? stock;

            try
            {
                response = await _finnhubService.GetStockPriceQuote(_stockOptions.Value.StockSymbol = "MSFT");
                ViewBag.isError = false;

                stock  = new Stock()
                {
                    StockSymbol = _stockOptions.Value.StockSymbol,
                    CurrentPrice = Convert.ToDouble(response["c"].ToString()),
                    HighestPrice = Convert.ToDouble(response["h"].ToString()),
                    LowestPrice = Convert.ToDouble(response["l"].ToString()),
                    OpenPirce = Convert.ToDouble(response["o"].ToString())
                };

                return View(stock);

            }
            catch (InvalidOperationException ie)
            {
                ViewBag.isError = true;
                ViewBag.Error = ie.StackTrace;
            }
            catch (Exception e)
            {
                ViewBag.isError = true;
                ViewBag.Error = e.StackTrace;
            }
            
            return View();
        }
    }
}
