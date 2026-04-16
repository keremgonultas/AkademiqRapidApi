using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public RecipeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("WeeklyRecipeCacheV3", out RecipeResponse recipeData))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://ai-food-recipe-generator-api-custom-diet-quick-meals.p.rapidapi.com/generate?noqueue=1"),
                    Headers =
                    {
                        { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
                        { "x-rapidapi-host", "ai-food-recipe-generator-api-custom-diet-quick-meals.p.rapidapi.com" },
                    }
                };

                var requestBody = new
                {
                    ingredients = new[] { "chicken", "rice", "pepper" },
                    dietary_restrictions = Array.Empty<string>(),
                    cuisine = "Italian",
                    meal_type = "dinner",
                    servings = 2,
                    lang = "en"
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                request.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    var jsonBody = response.Content.ReadAsStringAsync().Result;

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    recipeData = JsonSerializer.Deserialize<RecipeResponse>(jsonBody, options);

                    if (recipeData?.result != null)
                    {
                        recipeData.result.title = TranslationHelper.TranslateToTurkish(recipeData.result.title);

                        if (recipeData.result.ingredients != null)
                        {
                            foreach (var item in recipeData.result.ingredients)
                            {
                                item.name = TranslationHelper.TranslateToTurkish(item.name);
                                item.amount = TranslationHelper.TranslateToTurkish(item.amount);
                            }
                        }

                        if (recipeData.result.instructions != null)
                        {
                            for (int i = 0; i < recipeData.result.instructions.Count; i++)
                            {
                                recipeData.result.instructions[i] = TranslationHelper.TranslateToTurkish(recipeData.result.instructions[i]);
                            }
                        }

                        _memoryCache.Set("WeeklyRecipeCacheV3", recipeData, TimeSpan.FromDays(7));
                    }
                }
                catch
                {
                    recipeData = new RecipeResponse();
                }
            }

            return View(recipeData?.result);
        }
    }
}