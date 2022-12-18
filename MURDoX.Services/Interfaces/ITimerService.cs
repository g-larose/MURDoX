using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Interfaces
{
    public interface ITimerService
    {
        public void Start();
        public void Stop();
        public void Reset();
        public string GetStartDate();
        public string GetStartTime();
        public double GetTimerSeconds();
    }
}
