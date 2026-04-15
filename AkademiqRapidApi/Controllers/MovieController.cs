using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace AkademiqRapidApi.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult Index()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://imdb236.p.rapidapi.com/api/imdb/upcoming-releases?countryCode=TR&type=MOVIE"),
                Headers =
                {
                    { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
                    { "x-rapidapi-host", "imdb236.p.rapidapi.com" },
                },
            };

            var response = client.SendAsync(request).Result;
            var jsonBody = response.Content.ReadAsStringAsync().Result;

            // Veriyi ReleaseGroup listesi olarak çözümlüyoruz
            var values = JsonSerializer.Deserialize<List<MovieViewModel.ReleaseGroup>>(jsonBody);

            List<MovieViewModel.Title> allMovies = new List<MovieViewModel.Title>();

            // ... (üst kısımlar aynı)

            if (values != null)
            {
                allMovies = values.SelectMany(x => x.titles)
    // 1. ŞART: Afiş linki boş olmasın ve gerçek bir link (http) olsun
    .Where(x => !string.IsNullOrEmpty(x.primaryImage) && x.primaryImage.StartsWith("http"))

    // 2. ŞART: Bilinen bozuk afişli "Hokum" gibi filmleri ismen engelleyelim (Geçici ama kesin çözüm)
    .Where(x => x.primaryTitle != "Hokum" && x.primaryTitle != "Title Unknown")

    // 3. ŞART: Afiş linki çok kısaysa (mesela sadece "http://" gibi geliyorsa) onu da atalım
    .Where(x => x.primaryImage.Length > 15)

    .Where(x => DateTime.TryParse(x.releaseDate, out DateTime release) && release >= DateTime.Today)
    .OrderBy(x => x.releaseDate)
    .ToList();
            }

            return View(allMovies);
        }
    }
}