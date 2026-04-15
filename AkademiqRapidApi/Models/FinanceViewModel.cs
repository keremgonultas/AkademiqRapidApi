namespace AkademiqRapidApi.Models
{
    public class FinanceViewModel
    {
        public List<Article> body { get; set; }

        public class Article
        {
            public string title { get; set; }
            public string description { get; set; }
            public string link { get; set; }
            public string pubDate { get; set; }
        }
    }
}