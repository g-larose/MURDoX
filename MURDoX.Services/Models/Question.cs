using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
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
