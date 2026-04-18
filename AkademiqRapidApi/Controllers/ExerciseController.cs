using AkademiqRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace AkademiqRapidApi.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public ExerciseController(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("DailyExerciseCache", out List<ExerciseViewModel> exerciseList))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://exercisedb.p.rapidapi.com/exercises?limit=10"),
                    Headers =
                    {
                        { "x-rapidapi-key", _configuration["RapidApiKey"] },
                        { "x-rapidapi-host", "exercisedb.p.rapidapi.com" },
                    },
                };

                try
                {
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    var jsonBody = response.Content.ReadAsStringAsync().Result;

                    exerciseList = JsonSerializer.Deserialize<List<ExerciseViewModel>>(jsonBody);

                    if (exerciseList != null)
                    {
                        foreach (var item in exerciseList)
                        {
                            item.name = TranslationHelper.TranslateToTurkish(item.name);
                            item.bodyPart = TranslationHelper.TranslateToTurkish(item.bodyPart);
                            item.target = TranslationHelper.TranslateToTurkish(item.target);
                            item.equipment = TranslationHelper.TranslateToTurkish(item.equipment);
                            item.difficulty = TranslationHelper.TranslateToTurkish(item.difficulty);

                            for (int i = 0; i < item.instructions.Count; i++)
                            {
                                item.instructions[i] = TranslationHelper.TranslateToTurkish(item.instructions[i]);
                            }
                        }

                        _memoryCache.Set("DailyExerciseCache", exerciseList, TimeSpan.FromHours(24));
                    }
                }
                catch
                {
                    exerciseList = new List<ExerciseViewModel>();
                }
            }

            return View(exerciseList);
        }
    }
}