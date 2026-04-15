namespace AkademiqRapidApi.Models
{
    public class MovieViewModel
    {
        // JSON doğrudan bir liste olarak geldiği için Rootobject'e ihtiyacımız yok
        public class ReleaseGroup
        {
            public string date { get; set; }
            public Title[] titles { get; set; }
        }

        public class Title
        {
            public string id { get; set; }
            public string primaryTitle { get; set; }
            public string type { get; set; }
            public string description { get; set; }
            public string primaryImage { get; set; } // İşte aradığımız afiş linki!
            public string releaseDate { get; set; }
            public float? averageRating { get; set; } // Puan (Null gelebilme ihtimaline karşı ? koyduk)
            public string[] genres { get; set; }
        }
    }
}