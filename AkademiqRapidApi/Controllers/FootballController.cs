using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory; // Önbellekleme kütüphanesi

namespace AkademiqRapidApi.Controllers
{
    public class FootballController : Controller
    {
        // 1. Dependency Injection için değişkenimizi tanımlıyoruz
        private readonly IMemoryCache _memoryCache;

        // 2. Constructor (Yapıcı Metot) ile projeye eklediğimiz MemoryCache servisini çağırıyoruz
        public FootballController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // 3. Önce hafızada "SuperLigTablosu" adında bir veri var mı diye kontrol ediyoruz
            // Eğer yoksa (veya süresi dolmuşsa), if bloğunun içine girip API'ye istek atacak.
            if (!_memoryCache.TryGetValue("SuperLigTablosu", out List<FootballViewModel.TeamStanding> values))
            {
                // Hafızada yok, mecburen API'den güncel veriyi çekiyoruz
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://super-lig-standings.p.rapidapi.com/?season=2025"),
                    Headers =
                    {
                        { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
                        { "x-rapidapi-host", "super-lig-standings.p.rapidapi.com" },
                    }
                };

                var response = client.SendAsync(request).Result;
                var jsonBody = response.Content.ReadAsStringAsync().Result;

                values = JsonSerializer.Deserialize<List<FootballViewModel.TeamStanding>>(jsonBody);

                if (values != null)
                {
                    // Tabloyu sıralıyoruz
                    values = values.OrderBy(x => x.stats.rank).ToList();

                    // 4. API'den gelen bu yeni veriyi 1 SAAT (TimeSpan.FromHours(1)) boyunca hafızada tutması için ayarlıyoruz
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                    // 5. Veriyi "SuperLigTablosu" ismiyle RAM'e kaydediyoruz
                    _memoryCache.Set("SuperLigTablosu", values, cacheOptions);
                }
            }

            // 6. Sonucu View'a gönderiyoruz (Eğer 1 saat dolmadıysa API'ye hiç gitmeden direkt hafızadaki veriyi basacak)
            return View(values);
        }
    }
}