using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http;

namespace AkademiqRapidApi.Controllers
{
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                // Buraya RapidAPI'den aldığın tam URL'yi yapıştır
                RequestUri = new Uri("https://yahoo-finance15.p.rapidapi.com/api/v1/markets/news?ticker=AAPL%2CTSLA"),
                Headers =
                {
                    { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
                    { "x-rapidapi-host", "yahoo-finance15.p.rapidapi.com" },
                },
            };

            var response = client.SendAsync(request).Result;
            var jsonBody = response.Content.ReadAsStringAsync().Result;

            // JSON verisini modele aktarıyoruz
            var values = JsonSerializer.Deserialize<FinanceViewModel>(jsonBody);

            return View(values);
        }
    }
}