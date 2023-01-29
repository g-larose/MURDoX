using MURDoX.Data.Assets.Welcomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MURDoX.Services.Helpers
{
    public class WelcomerHelper
    {
        public static string GenerateWelcomeMessage()
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Welcomer", "welcome_messages.json");
            var json = File.ReadAllText(jsonPath);
            var messages = JsonSerializer.Deserialize<List<WelcomerJson>>(json)!;
            var rnd = new Random();
            var index = rnd.Next(0, messages.Count());
            var message = messages[index];
            return message.Message;
        }
    }
}
