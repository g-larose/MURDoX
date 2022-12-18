using MURDoX.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Helpers
{
    public class FactHelper
    {
        public static async Task<string> GetRandomFact()
        {
            string factJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "facts.json");
            Random rnd = new Random();
            
            var facts = await File.ReadAllTextAsync(factJson);
            var factList = JsonConvert.DeserializeObject<FactRoot>(facts);
            int index = rnd.Next(0, factList.Facts.Count);
            var fact = factList.Facts[index];

            return fact;

        }
    }
}
