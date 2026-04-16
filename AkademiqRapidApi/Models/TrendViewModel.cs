using System.Collections.Generic;

namespace AkademiqRapidApi.Models
{
    public class TrendResponse
    {
        public List<TrendVideo> data { get; set; }
    }

    public class TrendVideo
    {
        public string videoId { get; set; }
        public string title { get; set; }
        public string channelTitle { get; set; }
        public string viewCount { get; set; }
        public string publishedTimeText { get; set; }
        public string lengthText { get; set; }
        public List<TrendThumbnail> thumbnail { get; set; }
    }

    public class TrendThumbnail
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}