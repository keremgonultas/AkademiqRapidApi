namespace AkademiqRapidApi.Models
{
    public class WeatherViewModel
    {
        public Location location { get; set; }
        public Current_Observation current_observation { get; set; }
        public Forecast[] forecasts { get; set; }

        public class Location
        {
            public string city { get; set; }
            public string country { get; set; }
        }

        public class Current_Observation
        {
            public Wind wind { get; set; }
            public Atmosphere atmosphere { get; set; }
            public Astronomy astronomy { get; set; }
            public Condition condition { get; set; }
        }

        public class Wind { public int speed { get; set; } }
        public class Atmosphere { public int humidity { get; set; } }
        public class Astronomy { public string sunrise { get; set; } public string sunset { get; set; } }

        public class Condition
        {
            public string text { get; set; }
            public int temperature { get; set; }
        }

        public class Forecast
        {
            public string day { get; set; }
            public int low { get; set; }
            public int high { get; set; }
            public string text { get; set; }
        }
    }
}