using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class GasPriceController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public GasPriceController(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("GasPriceCache", out GasPriceResponse gasData))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://gas-price.p.rapidapi.com/europeanCountries"),
                    Headers =
                    {
                        { "x-rapidapi-key", _configuration["RapidApiKey"] },
                        { "x-rapidapi-host", "gas-price.p.rapidapi.com" },
                    },
                };

                try
                {
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    var jsonBody = response.Content.ReadAsStringAsync().Result;

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        IgnoreNullValues = true
                    };

                    gasData = JsonSerializer.Deserialize<GasPriceResponse>(jsonBody, options);

                    if (gasData?.result != null && gasData.result.Count > 0)
                    {
                        _memoryCache.Set("GasPriceCache", gasData, TimeSpan.FromHours(12));
                    }
                }
                catch
                {
                    gasData = new GasPriceResponse();
                }
            }

            return View(gasData?.result);
        }
    }
}