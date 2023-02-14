// using MURDoX.Core.Models;
// using Newtonsoft.Json;
//
// namespace MURDoX.Core.Helpers
// {
//     public class TriviaHttpHelper
//     {
//         public static List<Question> HandleQuestionResponse(string? response)
//         {
//             dynamic? quests = JsonConvert.DeserializeObject(response!);
//             List<Question> Questions = new();
//
//             if (quests == null) return Questions;
//             foreach (dynamic? quest in quests["results"])
//             {
//                 Question question = new();
//                 question.Category = quest.category;
//                 question._Question = quest.question;
//                 question.Type = quest.type;
//                 question.Difficulty = quest.difficulty;
//                 question.CorrectAnswer = quest.correct_answer;
//                 question.Answers = quest.incorrect_answers;
//                 Questions.Add(question);
//             }
//             return Questions;
//         }
//
//         public static async Task<string> MakeQuestionRequest(string category, string difficulty)
//         {
//             string cat = ConverterHelper.ConvertCategoryString(category);
//             string EndPoint = $"https://opentdb.com/api.php?amount=10&category={cat}&difficulty={difficulty}&type=multiple";
//             HttpClient request = new();
//
//             HttpResponseMessage response = await request.GetAsync(EndPoint);
//             string content = await response.Content.ReadAsStringAsync();
//             content = UtilityHelper.Sanitize(content);
//             return content;
//         }
//     }
// }
