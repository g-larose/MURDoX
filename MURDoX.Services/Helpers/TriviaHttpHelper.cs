using MURDoX.Services.Converters;
using MURDoX.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MURDoX.Services.Helpers
{
    public class TriviaHttpHelper
    {
        public static List<Question> HandleQuestionResponse(string? response)
        {
            dynamic? quests = JsonConvert.DeserializeObject(response!);
            List<Question> Questions = new();

            if (quests == null) return Questions;
            foreach (var quest in quests["results"])
            {
                Question question = new();
                question.Category = quest.category;
                question._Question = quest.question;
                question.Type = quest.type;
                question.Difficulty = quest.difficulty;
                question.CorrectAnswer = quest.correct_answer;
                question.Answers = quest.incorrect_answers;
                Questions.Add(question);
            }
            return Questions;
        }

        public static async Task<string> MakeQuestionRequest(string category, string difficulty)
        {
            var cat = Converter.ConvertCategoryString(category);
            var EndPoint = $"https://opentdb.com/api.php?amount=10&category={cat}&difficulty={difficulty}&type=multiple";
            HttpClient request = new HttpClient();

            HttpResponseMessage response = await request.GetAsync(EndPoint);
            string content = await response.Content.ReadAsStringAsync();
            content = Sanitize(content);
            return content;
        }

        private static string Sanitize(string content)
        {
            Regex matches = new Regex("(&#?[a-zA-Z0-9]+;)");
            string output = matches.Replace(content, "");

            return output;
        }
    }
}
