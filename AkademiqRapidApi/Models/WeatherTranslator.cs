namespace AkademiqRapidApi.Models
{
    public static class WeatherTranslator
    {
        // Hava Durumu Çevirisi
        public static string TranslateCondition(string condition)
        {
            if (string.IsNullOrEmpty(condition)) return "";

            var lower = condition.ToLower();

            if (lower.Contains("partly cloudy")) return "Parçalı Bulutlu";
            if (lower.Contains("mostly cloudy")) return "Çok Bulutlu";
            if (lower.Contains("cloudy")) return "Bulutlu";
            if (lower.Contains("sunny")) return "Güneşli";
            if (lower.Contains("clear") || lower.Contains("fair")) return "Açık";
            if (lower.Contains("rain") || lower.Contains("shower")) return "Yağmurlu";
            if (lower.Contains("snow")) return "Karlı";
            if (lower.Contains("storm") || lower.Contains("thunder")) return "Fırtınalı";
            if (lower.Contains("wind") || lower.Contains("breeze") || lower.Contains("breezy")) return "Rüzgarlı";
            if (lower.Contains("fog") || lower.Contains("haze")) return "Sisli";

            return condition; 
        }

        
        public static string TranslateDay(string day)
        {
            return day switch
            {
                "Mon" => "Pzt",
                "Tue" => "Sal",
                "Wed" => "Çar",
                "Thu" => "Per",
                "Fri" => "Cum",
                "Sat" => "Cmt",
                "Sun" => "Paz",
                "Monday" => "Pazartesi",
                "Tuesday" => "Salı",
                "Wednesday" => "Çarşamba",
                "Thursday" => "Perşembe",
                "Friday" => "Cuma",
                "Saturday" => "Cumartesi",
                "Sunday" => "Pazar",
                _ => day
            };
        }
    }
}