using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
{
    public class WelcomeMessageJson
    {
        [JsonProperty("Messages")]
        public List<string>? Messages { get; set; }
    }
}
