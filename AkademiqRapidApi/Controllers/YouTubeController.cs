using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class YouTubeController : Controller
    {
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return View();
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://youtube-mp36.p.rapidapi.com/dl?id={id}"),
                Headers =
    {
        { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
        { "x-rapidapi-host", "youtube-mp36.p.rapidapi.com" },
    },
            };

            var response = await client.SendAsync(request);

            var jsonBody = await response.Content.ReadAsStringAsync();

            var values = JsonSerializer.Deserialize<YouTubeViewModel.Rootobject>(jsonBody);



            return View(values);
        }
    }
}
