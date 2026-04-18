using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public WeatherController(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("WeatherCache", out WeatherViewModel values))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://yahoo-weather5.p.rapidapi.com/weather?location=%C4%B0stanbul&format=json&u=c"),
                    Headers =
                    {
                        { "x-rapidapi-key", _configuration["RapidApiKey"] },
                        { "x-rapidapi-host", "yahoo-weather5.p.rapidapi.com" },
                    },
                };

                try
                {
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    var jsonBody = response.Content.ReadAsStringAsync().Result;

                    values = JsonSerializer.Deserialize<WeatherViewModel>(jsonBody);

                    if (values != null && values.current_observation != null)
                    {
                        _memoryCache.Set("WeatherCache", values, TimeSpan.FromMinutes(30));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hava Durumu API Hatası: " + ex.Message);
                    values = new WeatherViewModel();
                }
            }

            return View(values);
        }
    }
}