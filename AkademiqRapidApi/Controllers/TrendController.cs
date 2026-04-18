using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class TrendController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public TrendController(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("TrendVideoCache", out TrendResponse trendData))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://yt-api.p.rapidapi.com/trending?geo=TR&type=now"),
                    Headers =
                    {
                        { "x-rapidapi-key", _configuration["RapidApiKey"] },
                        { "x-rapidapi-host", "yt-api.p.rapidapi.com" },
                    },
                };

                try
                {
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    var jsonBody = response.Content.ReadAsStringAsync().Result;

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    trendData = JsonSerializer.Deserialize<TrendResponse>(jsonBody, options);

                    if (trendData?.data != null)
                    {
                        _memoryCache.Set("TrendVideoCache", trendData, TimeSpan.FromHours(12));
                    }
                }
                catch
                {
                    trendData = new TrendResponse();
                }
            }

            return View(trendData?.data);
        }
    }
}