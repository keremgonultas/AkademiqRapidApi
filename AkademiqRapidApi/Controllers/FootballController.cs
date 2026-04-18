using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace AkademiqRapidApi.Controllers
{
    public class FootballController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public FootballController(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("LigCache", out List<FootballViewModel.TeamStanding> values))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://super-lig-standings.p.rapidapi.com/?season=2025"),
                    Headers =
                    {
                        { "x-rapidapi-key", _configuration["RapidApiKey"] },
                        { "x-rapidapi-host", "super-lig-standings.p.rapidapi.com" },
                    }
                };

                try
                {
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    var jsonBody = response.Content.ReadAsStringAsync().Result;

                    values = JsonSerializer.Deserialize<List<FootballViewModel.TeamStanding>>(jsonBody);

                    if (values != null && values.Count > 0)
                    {
                        values = values.OrderBy(x => x.stats.rank).ToList();
                        _memoryCache.Set("LigCache", values, TimeSpan.FromHours(1));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Süper Lig API Hatası: " + ex.Message);
                    values = new List<FootballViewModel.TeamStanding>();
                }
            }

            return View(values);
        }
    }
}