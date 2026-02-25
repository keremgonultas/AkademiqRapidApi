using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class GasController : Controller
    {
        public async Task<IActionResult> Index()
        {
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://gas-price.p.rapidapi.com/europeanCountries"),
                Headers =
    {
        { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
        { "x-rapidapi-host", "gas-price.p.rapidapi.com" },
    },
            };
            
            var response = await client.SendAsync(request);

            var jsonBody = await response.Content.ReadAsStringAsync();

            var values = JsonSerializer.Deserialize<GasViewModel.Rootobject>(jsonBody);

            // deserialize : json -> nesneye ceviriyor
            // serialize : nesne -> json ceviriyor

            return View(values);
        }
    }
}
