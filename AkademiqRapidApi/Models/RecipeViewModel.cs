using System.Collections.Generic;

namespace AkademiqRapidApi.Models
{
    public class RecipeResponse
    {
        public RecipeResult result { get; set; }
        public long cacheTime { get; set; }
        public long time { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }

    public class RecipeResult
    {
        public string title { get; set; }
        public List<RecipeIngredient> ingredients { get; set; }
        public List<string> instructions { get; set; }
        public RecipeNutrition nutrition_info { get; set; }
    }

    public class RecipeIngredient
    {
        public string name { get; set; }
        public string amount { get; set; }
    }

    public class RecipeNutrition
    {
        public int calories { get; set; }
        public string protein { get; set; }
        public string carbs { get; set; }
        public string fats { get; set; }
    }
}