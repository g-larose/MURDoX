using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Interfaces
{
    public interface ILoggerService
    {
        void Save(Log log);
        List<Log> GetAll();
        Log GetLog(Guid id);
        void Delete(Guid id);
    }
}
