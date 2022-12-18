using MURDoX.Data.Assets;
using MURDoX.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Interfaces
{
    public interface IDataService
    {
        ConfigJson GetApplicationConfig();
        FactsJson LoadFactsJson();
        Task<T> LoadJsonAsync<T>(string path);
    }
}
