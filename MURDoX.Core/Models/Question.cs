using Newtonsoft.Json.Linq;

namespace MURDoX.Core.Models
{
    public class Question
    {
        public string? Category { get; set; }
        public string? _Question { get; set; }
        public string? CorrectAnswer { get; set; }
        public string? Difficulty { get; set; }
        public string? Type { get; set; }
        public JArray? Answers { get; set; }
    }
}