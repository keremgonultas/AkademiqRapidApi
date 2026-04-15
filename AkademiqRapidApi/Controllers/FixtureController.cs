using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;

namespace AkademiqRapidApi.Controllers
{
    public class FixtureController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public FixtureController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index(int? selectedWeek)
        {
            // --- 1. ADIM: TAKIM LOGOLARINI ÇEKELİM (PUAN DURUMU API'SİNDEN) ---
            if (!_memoryCache.TryGetValue("TeamLogos", out Dictionary<string, string> teamLogos))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://super-lig-standings.p.rapidapi.com/?season=2025"),
                    Headers = {
                        { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
                        { "x-rapidapi-host", "super-lig-standings.p.rapidapi.com" },
                    }
                };
                var response = client.SendAsync(request).Result;
                var json = response.Content.ReadAsStringAsync().Result;

                // Güvenlik kontrolü
                if (!string.IsNullOrWhiteSpace(json) && json.StartsWith("["))
                {
                    var standings = JsonSerializer.Deserialize<List<FootballViewModel.TeamStanding>>(json);
                    teamLogos = standings.ToDictionary(x => x.team.name, x => x.team.logo);
                    _memoryCache.Set("TeamLogos", teamLogos, TimeSpan.FromHours(24));
                }
                else
                {
                    teamLogos = new Dictionary<string, string>(); // Çökmeyi engellemek için boş liste
                }
            }
            ViewBag.Logos = teamLogos;

            // --- 2. ADIM: FİKSTÜR VERİSİNİ ÇEKELİM ---
            if (!_memoryCache.TryGetValue("FiksturVerisi", out FixtureViewModel matchData))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://sofascore.p.rapidapi.com/tournaments/get-last-matches?tournamentId=52&seasonId=77805&pageIndex=0"),
                    Headers = {
                        { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
                        
                        // DÜZELTME: Host kısmındaki "http://" ve sondaki "/" işaretini kaldırdım!
                        { "x-rapidapi-host", "sofascore.p.rapidapi.com" },
                    }
                };

                var response = client.SendAsync(request).Result;
                var json = response.Content.ReadAsStringAsync().Result;

                // --- SUÇÜSTÜ (DEBUG) MEKANİZMASI ---
                System.Diagnostics.Debug.WriteLine("=========================================");
                System.Diagnostics.Debug.WriteLine("🚨 FİKSTÜR API'DEN GELEN HAM CEVAP: ");
                System.Diagnostics.Debug.WriteLine(json);
                System.Diagnostics.Debug.WriteLine("=========================================");

                // Gelen metin gerçekten bir JSON formatı mı diye kontrol ediyoruz
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
                    // API patladıysa veya kota bittiyse sayfayı çökertme, boş sayfa göster
                    matchData = new FixtureViewModel { events = new List<FixtureViewModel.Event>() };
                }
            }

            // Eğer API'den null geldiyse veya kota dolduysa boş ekranı döndür
            if (matchData == null || matchData.events == null || !matchData.events.Any())
            {
                ViewBag.AllWeeks = new List<int>();
                ViewBag.SelectedWeek = 0;
                return View(new List<FixtureViewModel.Event>());
            }

            // --- 3. ADIM: HAFTA FİLTRELEME (ROUNDINFO KORUMASI EKLENDİ) ---
            var allWeeks = matchData.events
                .Where(x => x.roundInfo != null) // roundInfo boş (null) gelen maçlar sistemi çökertmesin
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