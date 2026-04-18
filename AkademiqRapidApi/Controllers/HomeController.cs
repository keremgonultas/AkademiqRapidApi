using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace AkademiqRapidApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public HomeController(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var dashboardData = new DashboardViewModel();

            if (_memoryCache.TryGetValue("WeatherCache", out WeatherViewModel weather))
            {
                dashboardData.WeatherInfo = weather;
            }

            if (_memoryCache.TryGetValue("CryptoDataCacheV2", out CryptoResponse crypto))
            {
                dashboardData.TopCryptoCoins = crypto?.data?.coins?.Take(4).ToList();
            }

            if (_memoryCache.TryGetValue("GasPriceCache", out GasPriceResponse gas))
            {
                dashboardData.TurkeyGasPrice = gas?.result?.FirstOrDefault(x => x.country == "Turkey");
            }

            if (_memoryCache.TryGetValue("TrendVideoCache", out TrendResponse trend))
            {
                dashboardData.TrendVideos = trend?.data?.Take(2).ToList();
            }

            if (_memoryCache.TryGetValue("LigCache", out List<FootballViewModel.TeamStanding> football))
            {
                dashboardData.TopTeams = football?.Take(3).ToList();
            }

            return View(dashboardData);
        }
    }
}