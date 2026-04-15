using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;

namespace AkademiqRapidApi.Controllers
{
    public class FootballController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
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

            
            var values = JsonSerializer.Deserialize<List<FootballViewModel.TeamStanding>>(jsonBody);

            
            if (values != null)
            {
                values = values.OrderBy(x => x.stats.rank).ToList();
            }

            return View(values);
        }
    }
}