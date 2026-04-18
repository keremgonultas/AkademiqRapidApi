using System.Collections.Generic;

namespace AkademiqRapidApi.Models
{
    public class CryptoResponse
    {
        public string status { get; set; }
        public CryptoData data { get; set; }
    }

    public class CryptoData
    {
        public List<CryptoCoin> coins { get; set; }
    }

    public class CryptoCoin
    {
        public string uuid { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string iconUrl { get; set; }
        public string marketCap { get; set; }
        public string price { get; set; }
        public string change { get; set; }
        public int rank { get; set; }
        public List<string> sparkline { get; set; }
        public string coinrankingUrl { get; set; }
        public string btcPrice { get; set; }
    }
}