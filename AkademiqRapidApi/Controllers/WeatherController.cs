using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index( )
        {

            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://open-weather13.p.rapidapi.com/city?city=istanbul&lang=TR"),
                Headers =
    {
        { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
        { "x-rapidapi-host", "open-weather13.p.rapidapi.com" },
    },
            };
           
            var response = client.SendAsync(request).Result;

            var jsonBody = response.Content.ReadAsStringAsync().Result;

            var values = JsonSerializer.Deserialize<WeatherViewModel.Rootobject>(jsonBody);
            
            return View(values);
        }
    }
}
