using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AkademiqRapidApi.Models
{
    public class GasPriceResponse
    {
        [JsonPropertyName("success")]
        public bool success { get; set; }

        [JsonPropertyName("result")]
        public List<GasPriceItem> result { get; set; }
    }

    public class GasPriceItem
    {
        [JsonPropertyName("currency")]
        public string currency { get; set; }

        [JsonPropertyName("lpg")]
        public string lpg { get; set; }

        [JsonPropertyName("diesel")]
        public string diesel { get; set; }

        [JsonPropertyName("gasoline")]
        public string gasoline { get; set; }

        [JsonPropertyName("country")]
        public string country { get; set; }
    }
}