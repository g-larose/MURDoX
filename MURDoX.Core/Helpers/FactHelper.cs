﻿using MURDoX.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MURDoX.Core.Models;

namespace MURDoX.Services.Helpers
{
    public class FactHelper
    {
        public static async Task<Fact> GetRandomFact()
        {
            string factJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "fact.json");
            Random rnd = new Random();
            
            var facts = await File.ReadAllTextAsync(factJson);
            var factList = JsonConvert.DeserializeObject<List<Fact>>(facts);
            int index = rnd.Next(0, factList!.Count);
            var fact = factList[index];
            fact.Content = UtilityHelper.Sanitize(fact.Content);

            return fact;

        }
    }
}
