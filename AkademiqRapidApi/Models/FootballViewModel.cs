namespace AkademiqRapidApi.Models
{
    public class FootballViewModel
    {
        
        public class TeamStanding
        {
            public Team team { get; set; }
            public Stats stats { get; set; }
        }

        public class Team
        {
            public string name { get; set; }
            public string logo { get; set; }
            public string abbreviation { get; set; }
        }

        public class Stats
        {
            public int wins { get; set; }
            public int losses { get; set; }
            public int ties { get; set; }
            public int gamesPlayed { get; set; }
            public int goalsFor { get; set; }
            public int goalsAgainst { get; set; }
            public int points { get; set; }
            public int rank { get; set; }
            public int goalDifference { get; set; }
        }
    }
}