using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class MovieController : Controller
    {
        private readonly IConfiguration _configuration;

        public MovieController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://imdb236.p.rapidapi.com/api/imdb/upcoming-releases?countryCode=TR&type=MOVIE"),
                Headers =
                {
                    { "x-rapidapi-key", _configuration["RapidApiKey"] },
                    { "x-rapidapi-host", "imdb236.p.rapidapi.com" },
                },
            };

            var response = client.SendAsync(request).Result;
            var jsonBody = response.Content.ReadAsStringAsync().Result;

            var values = JsonSerializer.Deserialize<List<MovieViewModel.ReleaseGroup>>(jsonBody);

            List<MovieViewModel.Title> allMovies = new List<MovieViewModel.Title>();

            if (values != null)
            {
                allMovies = values.SelectMany(x => x.titles)
                    .Where(x => !string.IsNullOrEmpty(x.primaryImage) && x.primaryImage.StartsWith("http"))
                    .Where(x => x.primaryTitle != "Hokum" && x.primaryTitle != "Title Unknown")
                    .Where(x => x.primaryImage.Length > 15)
                    .Where(x => DateTime.TryParse(x.releaseDate, out DateTime release) && release >= DateTime.Today)
                    .OrderBy(x => x.releaseDate)
                    .ToList();
            }

            return View(allMovies);
        }
    }
}