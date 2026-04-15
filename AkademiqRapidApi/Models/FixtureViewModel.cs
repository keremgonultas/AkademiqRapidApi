namespace AkademiqRapidApi.Models
{
    public class FixtureViewModel
    {
        public List<Event> events { get; set; }

        public class Event
        {
            public Tournament tournament { get; set; }
            public RoundInfo roundInfo { get; set; }
            public Status status { get; set; }
            public Team homeTeam { get; set; }
            public Team awayTeam { get; set; }
            public Score homeScore { get; set; }
            public Score awayScore { get; set; }
            public long startTimestamp { get; set; }
        }

        public class Tournament { public string name { get; set; } }

        public class RoundInfo { public int round { get; set; } }

        public class Status
        {
            public int code { get; set; } // Hata buradaydı, ekledik
            public string description { get; set; }
            public string type { get; set; }
        }

        public class Team
        {
            public string name { get; set; }
            public string shortName { get; set; }
            public TeamColors teamColors { get; set; } // Hata buradaydı, ekledik
        }

        // Takım renkleri için yeni sınıf
        public class TeamColors
        {
            public string primary { get; set; }
            public string secondary { get; set; }
            public string text { get; set; }
        }

        public class Score { public int? current { get; set; } }
    }
}