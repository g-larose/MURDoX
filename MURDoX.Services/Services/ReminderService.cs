using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Services
{
    public class ReminderService
    {
        private readonly TimerService _timerService;
        private string _reminder;

        public ReminderService(TimerService timerService)
        {
            _timerService = timerService;
        }

        public async Task<string> SetReminder(TimeSpan duration, string reminder)
        {
            return "";
        }


    }
}
