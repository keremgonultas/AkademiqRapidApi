using System.Collections.Generic;

namespace AkademiqRapidApi.Models
{
    public class ExerciseViewModel
    {
        public string bodyPart { get; set; }
        public string equipment { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string target { get; set; }
        public List<string> secondaryMuscles { get; set; }
        public List<string> instructions { get; set; }
        public string description { get; set; }
        public string difficulty { get; set; }
        public string category { get; set; }
    }
}