using MURDoX.Core.Models;
using Newtonsoft.Json;

namespace MURDoX.Core.Helpers
{
    public class FactHelper
    {
        public static async Task<Fact> GetRandomFact()
        {
            string factJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "fact.json");
            Random rnd = new();
            
            string facts = await File.ReadAllTextAsync(factJson);
            List<Fact>? factList = JsonConvert.DeserializeObject<List<Fact>>(facts);
            int index = rnd.Next(0, factList!.Count);
            Fact fact = factList[index];
            fact.Content = UtilityHelper.Sanitize(fact.Content);

            return fact;

        }
    }
}
