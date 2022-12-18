using MURDoX.Data.Assets;
using MURDoX.Data.Configuration;
using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Services
{
    public class DataService : IDataService
    {

        public ConfigJson GetApplicationConfig()
        {
            var configFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "/Configuration/config.json");
            using var fs = File.OpenRead(configFile);
            using var sr = new StreamReader(fs, new UTF8Encoding(false));
            string json = sr.ReadToEnd();

            ConfigJson? configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            return configJson!;
        }

        public FactsJson LoadFactsJson()
        {
            var factsFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "/Assets/facts.json");
            using var fs = File.OpenRead(factsFile);
            using var sr = new StreamReader(fs, new UTF8Encoding(false));
            string json = sr.ReadToEnd();

            FactsJson? factsJson = JsonConvert.DeserializeObject<FactsJson>(json);

            return factsJson!;
        }

        public async Task<T> LoadJsonAsync<T>(string path)
        {
            var json = await File.ReadAllTextAsync(path);
            var genericJson = JsonConvert.DeserializeObject<T>(json);
            return genericJson!;
        }
    }
}
