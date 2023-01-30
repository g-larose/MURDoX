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

        public double ConvertMillisecondsToSeconds(double milliseconds)
        {
            throw new NotImplementedException();
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

        public static TimerModel GetBotUpTime()
        {
            var seconds = Timer!.Elapsed.Seconds;
            var Minutes = Timer.Elapsed.Minutes;
            var hours = Timer.Elapsed.Hours;
            var days = Timer.Elapsed.Days;
            var weeks = (days % 365) / 7;
            var months = (days % 365) / 12;
            var years = (days / 365);
            days -= ((years * 365) + (weeks * 7));

            var uptime = new TimerModel(seconds, Minutes, hours, days, weeks, months, years);
            return uptime;
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
