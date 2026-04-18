using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class CryptoController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public CryptoController(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("CryptoDataCacheV2", out CryptoResponse cryptoData))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://coinranking1.p.rapidapi.com/coins/trending?referenceCurrencyUuid=yhjMzLPhuIDl&timePeriod=24h&limit=25&offset=0"),
                    Headers =
                    {
                        { "x-rapidapi-key", _configuration["RapidApiKey"] },
                        { "x-rapidapi-host", "coinranking1.p.rapidapi.com" },
                    },
                };

                try
                {
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    var jsonBody = response.Content.ReadAsStringAsync().Result;

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        IgnoreNullValues = true
                    };

                    cryptoData = JsonSerializer.Deserialize<CryptoResponse>(jsonBody, options);

                    if (cryptoData?.data?.coins != null && cryptoData.data.coins.Count > 0)
                    {
                        _memoryCache.Set("CryptoDataCacheV2", cryptoData, TimeSpan.FromMinutes(5));
                    }
                }
                catch
                {
                    cryptoData = new CryptoResponse();
                }
            }

            return View(cryptoData?.data?.coins);
        }
    }
}