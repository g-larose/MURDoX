using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Recognizers.Text.DateTime;
using NodaTime;
using Recognizers.Text.DateTime.Wrapper.Models.BclDateTime;
using Remora.Commands.Results;
using Remora.Results;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MURDoX.Services.Helpers
{
    public sealed class TimerServiceHelper
    {
        private static readonly Regex _timeRegex = new(@"(?<quantity>\d+)(?<unit>mo|[ywdhms])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const string ReminderTimeNotPresent = "It seems you didn't specify a time in your reminder.\n" +
                                                      "I can recognize times like 10m, 5h, 2h30m, and even natural language like 'three hours from now' and 'in 2 days'";
        private static readonly TimeSpan _buffer = TimeSpan.FromSeconds(2);

        private static IDateTimeZoneProvider _timezones;
        private readonly DiscordClient _client;

        public TimerServiceHelper(DiscordClient client)
        {
            _client= client;
        }

        public static Result<TimeSpan> TryParse(string input)
        {
            var start = TimeSpan.Zero;
            var matches = _timeRegex.Matches(input);

            if (!matches.Any()) 
                 Result<TimeSpan>.FromError(new ParsingError<TimeSpan>(input, "Failed to extract time from input."));
          

              var returnResult = matches.Aggregate(start, (c, n) =>
              {
                  var multiplier = int.Parse(n.Groups["quantity"].Value);
                  var unit = n.Groups["unit"].Value;

                  return c + unit switch
                  {
                      "mo" => TimeSpan.FromDays(30 * multiplier),
                      "y" => TimeSpan.FromDays(365 * multiplier),
                      "w" => TimeSpan.FromDays(7 * multiplier),
                      "d" => TimeSpan.FromDays(multiplier),
                      "h" => TimeSpan.FromHours(multiplier),
                      "m" => TimeSpan.FromMinutes(multiplier),
                      "s" => TimeSpan.FromSeconds(multiplier),
                      _ => TimeSpan.Zero
                  };
              });

            if (returnResult == TimeSpan.Zero)
                return Result<TimeSpan>.FromError(new ParsingError<TimeSpan>(input, "Failed to extract time from input."));

            return Result<TimeSpan>.FromSuccess(returnResult);
        }

        public static Result<TimeSpan> ExtractTime(string input, Offset? offset, out string remainder)
        {
            remainder = input;

            if (string.IsNullOrEmpty(input))
                throw new InvalidOperationException("Cannot extract time from empty string.");

            if (TryParse(input.Split(' ')[0]).IsDefined(out var basicTime))
                return Result<TimeSpan>.FromSuccess(basicTime);

            var currentYear = DateTime.UtcNow.Year;
            var options = DateTimeOptions.None;
            var refTime = DateTime.UtcNow + (offset?.ToTimeSpan() ?? TimeSpan.Zero);
            var parsedTimes = DateTimeRecognizer.RecognizeDateTime(input, CultureInfo.InvariantCulture.DisplayName, options, refTime);

            if (parsedTimes.FirstOrDefault() is not { } parsedTime || !parsedTime.Resolution.Values.Any())
                return Result<TimeSpan>.FromError(new NotFoundError(ReminderTimeNotPresent));

            var timeModel = parsedTime
                           .Resolution
                           .Values
                           .Where(v => v is DateTimeV2Date or DateTimeV2DateTime)
                           .FirstOrDefault
                           (
                             v => v is DateTimeV2Date dtd
                                 ? dtd.Value.Year >= currentYear
                                 : (v as DateTimeV2DateTime)!.Value.Year >= currentYear
                           );

            if (timeModel is null)
                return Result<TimeSpan>.FromError(new NotFoundError(ReminderTimeNotPresent));

            remainder = (input[..parsedTime.Start] + input[(parsedTime.End + 1)..]).Trim();

            return timeModel is DateTimeV2Date vd
                ? (vd.Value - refTime).Add(_buffer)
                : ((timeModel as DateTimeV2DateTime)!.Value - refTime).Add(_buffer);
        }

        public async Task<Offset?> GetOffsetForUserAsync(ulong userId)
        {
            
            var user = await _client.GetUserAsync(userId);
            
            if (user is not null)
            {
                var date = DateTime.UtcNow;
                ZonedDateTime zDateTime = ZonedDateTime.FromDateTimeOffset(date);
                var offset = zDateTime.Offset;
                return offset;
            }

            return null;
        }
    }

}
