using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MURDoX.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core
{
    public class Startup
    {
        private readonly IHost _host;
        public Startup()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {

            }).Build();
        }
   
    }
}
