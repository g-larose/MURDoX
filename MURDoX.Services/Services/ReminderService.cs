using DSharpPlus;
using DSharpPlus.Entities;
using Humanizer;
using Humanizer.Localisation;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using NodaTime.Extensions;
using Remora.Results;
using System.Threading.Channels;

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
            var embed = new Embed();
            var embedBuilder = new EmbedBuilderHelper();
            if (_duration.IsSuccess)
            {
                var reminderTime = _duration.Entity.Humanize(1, minUnit: TimeUnit.Minute, maxUnit: TimeUnit.Year);
                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"you asked me to remind you **{_reminder}**, {reminderTime} ago.",
                };
                if (stopTime == _duration.Entity.TotalMinutes)
                {
                    _running = false;
                    await _channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                    await _user.SendMessageAsync(embed: embedBuilder.Build(embed));
                    _reminderTimer.Stop();
                    _reminderTimer.Close();
                    
                }
            }
            else
            {
                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = "Failed to initialize Reminder Service, Please try again later",
                };
                await _channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                _reminderTimer.Stop();
                _reminderTimer.Close();
            }
          
        }     

    }
}
