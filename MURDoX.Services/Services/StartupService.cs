using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MURDoX.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Services
{
    public class StartupService : IStartup
    {
        public Task<IHost> CreateHostBuilder(IServiceCollection servicCollection)
        {
            throw new NotImplementedException();
        }
    }
}
