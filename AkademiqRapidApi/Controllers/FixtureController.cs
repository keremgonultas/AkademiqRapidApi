using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class FixtureController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public FixtureController(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public IActionResult Index(int? selectedWeek)
        {
            if (!_memoryCache.TryGetValue("TeamLogos", out Dictionary<string, string> teamLogos))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://super-lig-standings.p.rapidapi.com/?season=2025"),
                    Headers = {
                        { "x-rapidapi-key", _configuration["RapidApiKey"] },
                        { "x-rapidapi-host", "super-lig-standings.p.rapidapi.com" },
                    }
                };
                var response = client.SendAsync(request).Result;
                var json = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrWhiteSpace(json) && json.StartsWith("["))
                {
                    var standings = JsonSerializer.Deserialize<List<FootballViewModel.TeamStanding>>(json);
                    teamLogos = standings.ToDictionary(x => x.team.name, x => x.team.logo);
                    _memoryCache.Set("TeamLogos", teamLogos, TimeSpan.FromHours(24));
                }
                else
                {
                    teamLogos = new Dictionary<string, string>();
                }
            }
            ViewBag.Logos = teamLogos;

            if (!_memoryCache.TryGetValue("FiksturVerisi", out FixtureViewModel matchData))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://sofascore.p.rapidapi.com/tournaments/get-last-matches?tournamentId=52&seasonId=77805&pageIndex=0"),
                    Headers = {
                        { "x-rapidapi-key", _configuration["RapidApiKey"] },
                        { "x-rapidapi-host", "sofascore.p.rapidapi.com" },
                    }
                };

                var response = client.SendAsync(request).Result;
                var json = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrWhiteSpace(json) && json.TrimStart().StartsWith("{"))
                {
                    matchData = JsonSerializer.Deserialize<FixtureViewModel>(json);

                    if (matchData != null && matchData.events != null)
                    {
                        _memoryCache.Set("FiksturVerisi", matchData, TimeSpan.FromHours(1));
                    }
                }
                else
                {
                    matchData = new FixtureViewModel { events = new List<FixtureViewModel.Event>() };
                }
            }

            if (matchData == null || matchData.events == null || !matchData.events.Any())
            {
                ViewBag.AllWeeks = new List<int>();
                ViewBag.SelectedWeek = 0;
                return View(new List<FixtureViewModel.Event>());
            }

            var allWeeks = matchData.events
                .Where(x => x.roundInfo != null)
                .Select(x => x.roundInfo.round)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.AllWeeks = allWeeks;
            int activeWeek = selectedWeek ?? (allWeeks.Any() ? allWeeks.Max() : 0);
            ViewBag.SelectedWeek = activeWeek;

            var filteredMatches = matchData.events
                .Where(x => x.roundInfo != null && x.roundInfo.round == activeWeek)
                .ToList();

            return View(filteredMatches);
        }
    }
}