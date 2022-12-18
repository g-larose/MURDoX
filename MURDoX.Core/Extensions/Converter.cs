using DSharpPlus.Entities;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Extensions
{
    public static class Converter
    {
        public static double ToHours(this long milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).TotalHours;
        }
        public static double ToMinutes(this long milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).TotalMinutes;
        }

        public static DiscordColor CategoryToColor(this string input)
        {
            input = input.ToLower();
            var result = input switch
            {
                "science" => EmbedColors.GetColor("orange", DiscordColor.Orange),
                "art" => EmbedColors.GetColor("purple", DiscordColor.Purple),
                _ => EmbedColors.GetColor("gray", DiscordColor.Gray)

            };
            return result;
        }
    }
}
