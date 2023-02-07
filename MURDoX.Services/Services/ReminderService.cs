using DSharpPlus;
using DSharpPlus.Entities;
using MURDoX.Services.Helpers;
using Remora.Results;

namespace MURDoX.Services.Services
{
    public class ReminderService
    {
        private string _reminder;
        private Result<TimeSpan> _duration;
        private double timerTickCount = 0;
        private bool _running;
        System.Timers.Timer _reminderTimer = new();
        DiscordChannel _channel;
        DiscordMember _user;

        public ReminderService(DiscordChannel channel, DiscordMember user, Result<TimeSpan> duration, string reminder)
        {
            _running = true;
            _user = user;
            _channel = channel;
            _reminder = reminder;
            _duration = duration;
            _reminderTimer.Interval = 1000;
            _reminderTimer.Elapsed += OnTimerElapsed;
            _reminderTimer.AutoReset = true;
            _reminderTimer.Enabled = true;

        }

        private async void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {

            timerTickCount++;
            var stopTime = TimerService.ConvertSecondsToMinutes(timerTickCount);
            // var stopTime = _duration.Entity.TotalMinutes;

            if (timerTickCount == 60)
            {
                    var test = "";
            }
                   
            if (stopTime == _duration.Entity.TotalMinutes)
            {
                _running = false;
               await  _channel.SendMessageAsync($"reminding {_user.Mention} to {_reminder} from {_duration.Entity.TotalMinutes} minutes ago!");
               await  _user.SendMessageAsync($"you asked me to remind you {_reminder} {_duration.Entity.TotalMinutes} minutes ago");
                _reminderTimer.Stop();
            }

            
                //success
        }

    }
}
