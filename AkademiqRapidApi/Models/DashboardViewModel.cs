using System.Collections.Generic;

namespace AkademiqRapidApi.Models
{
    public class DashboardViewModel
    {
        public WeatherViewModel WeatherInfo { get; set; }
        public List<CryptoCoin> TopCryptoCoins { get; set; }
        public List<FootballViewModel.TeamStanding> TopTeams { get; set; }
        public GasPriceItem TurkeyGasPrice { get; set; }
        public List<TrendVideo> TrendVideos { get; set; }
    }
}