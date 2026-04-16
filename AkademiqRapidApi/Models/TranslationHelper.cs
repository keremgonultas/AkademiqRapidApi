using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AkademiqRapidApi.Models
{
    public static class TranslationHelper
    {
        public static string TranslateToTurkish(string englishText)
        {
            if (string.IsNullOrWhiteSpace(englishText)) return "";

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://translateai.p.rapidapi.com/google/translate/text"),
                Headers =
                {
                    { "x-rapidapi-key", "fab1dc0c34msha15b0520af426b0p15289fjsndb95a722c59f" },
                    { "x-rapidapi-host", "translateai.p.rapidapi.com" },
                }
            };

            var requestBody = new
            {
                origin_language = "en",
                target_language = "tr",
                input_text = englishText
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                var jsonResponse = response.Content.ReadAsStringAsync().Result;

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;

                    if (root.TryGetProperty("translation", out JsonElement translationElement))
                    {
                        return translationElement.GetString();
                    }

                    return englishText;
                }
            }
            catch
            {
                return englishText;
            }
        }
    }
}