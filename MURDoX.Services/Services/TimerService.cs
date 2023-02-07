using Humanizer;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MURDoX.Extentions;
using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Services
{
    public class TimerService : ITimerService, IDisposable
    {
        public static Stopwatch? Timer { get; set; }
        
        public static string? StartDate { get; set; }
        public static string? StartTime { get; set; }

        public TimerService()
        {
            Start();
            
            
        }

        public string GetStartDate()
        {
            return StartDate!;
        }

        public string GetStartTime()
        {
            return StartTime!;
        }

        public void Reset()
        {
            if (Timer.IsRunning)
            {
                Stop();
                Timer = null;
                StartTime = "";
                StartDate = "";
            }
            else
            {
                Timer = null;
                StartTime = "";
                StartDate = "";
            }
        }

        public void Start()
        {
            Timer = new Stopwatch();
            Timer.Start();
            StartTime = DateTime.Now.ToShortTimeString();
            StartDate = DateTime.Now.ToLongDateString();
        }

        public void Stop()
        {
            Timer.Stop();
            Reset();
        }

        public static double ConvertMillisecondsToSeconds(double milliseconds)
        {
            return Math.Round(milliseconds / 1000);
        }

        public static double ConvertMillisecondsToMinutes(double milliseconds)
        {
            return Math.Round(milliseconds / 60000);
        }

        public static double ConvertMillisecondsToHours(double milliseconds)
        {
            return milliseconds / (1000 * 60 * 60);
        }

        public static double ConvertSecondsToMinutes(double seconds)
        {
            return seconds / 60;
        }

        public double GetTimerSeconds()
        {
            var seconds = Timer!.Elapsed.TotalSeconds;
            return seconds;
        }

        public string GetServerUptime()
        {
            var seconds = Timer!.Elapsed.Seconds;
            var Minutes = Timer.Elapsed.Minutes;
            var hours = Timer.Elapsed.Hours;
            var days = Timer.Elapsed.Days;
            var weeks = (days % 365) / 7;
            var years = (days / 365);
            days -= ((years * 365) + (weeks * 7));
            var uptime = String.Format("[{0}]:weeks [{1}]:days [{2}]:hours [{3}]:minutes [{4}]:seconds", weeks, days, hours, Minutes, seconds);
            return uptime;
        }

        public static string GetBotUpTime()
        {
            using var process  = Process.GetCurrentProcess();
            var seconds = Timer!.Elapsed.Seconds;
            var Minutes = Timer.Elapsed.Minutes;
            var hours = Timer.Elapsed.Hours;
            var days = Timer.Elapsed.Days;
            var weeks = (days % 365) / 7;
            var months = (days % 365) / 12;
            var years = (days / 365);
            days -= ((years * 365) + (weeks * 7));

            var uptime = new TimerModel(seconds, Minutes, hours, days, weeks, months, years);
            var uptime1 = "";
            try
            {
                var newTime = StartTime.Replace(":", ".").Replace("AM", string.Empty).Replace("PM", string.Empty).Trim();
                var uptimeDate = double.Parse(newTime);
                uptime1 = $"{DateTimeOffset.UtcNow.Subtract(process.StartTime).Humanize(2, minUnit: TimeUnit.Minute, maxUnit: TimeUnit.Year)}";
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            return uptime1;
        }

        public string GetTimerStartDate()
        {
            return StartDate!;
        }

        public void Dispose()
        {
            Stop();
            Reset();
            this.Dispose();
        }

    }
}
