namespace AkademiqRapidApi.Models
{
    public class MovieViewModel
    {
       
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
            public string primaryImage { get; set; } 
            public string releaseDate { get; set; }
            public float? averageRating { get; set; } 
            public string[] genres { get; set; }
        }
    }
}