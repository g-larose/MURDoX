using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MURDoX.Services.Helpers
{
    public class GameHelper
    {
        public static string Generate_8Ball_Saying()
        {
            var _8BallJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "8Ball", "sayings.json");
            var sayingsJson = File.ReadAllText(_8BallJsonPath);
            var saying = "";
            try
            {
                var sayingsList = JsonSerializer.Deserialize<_8BallRoot>(sayingsJson)!;
                var seed = DateTime.Now.Ticks;
                var rnd = new Random((int)seed);

                var index = rnd.Next(0, sayingsList.Sayings.Count());
                saying = sayingsList.Sayings[index];
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            

            return saying;
        }
    }
}
