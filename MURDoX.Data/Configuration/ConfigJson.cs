using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Data.Configuration
{
    public class ConfigJson
    {
        [JsonProperty("Token")]
        public string? Token { get; set; }
        [JsonProperty("Prefix")]
        public string[]? Prefix { get; set; }
        [JsonProperty("ConnectionString")]
        public string? ConnectionString { get; set; }
    }
}
