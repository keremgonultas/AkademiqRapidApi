using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class ConvertCurrencyController : Controller
    {
        public async Task<IActionResult>  Index()
        {
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://fast-price-exchange-rates.p.rapidapi.com/api/v1/convert?amount=1&base_currency=EUR&quote_currency=TRY"),
                Headers =
    {
        { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
        { "x-rapidapi-host", "fast-price-exchange-rates.p.rapidapi.com" },
    },
            };
            
            var response = await client.SendAsync(request);

            var jsonBody = await response.Content.ReadAsStringAsync();

            var values = JsonSerializer.Deserialize<ConvertCurrencyViewModel.Rootobject>(jsonBody);
            
            return View(values);
        }
    }
}
