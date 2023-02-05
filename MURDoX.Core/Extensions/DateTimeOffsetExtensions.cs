using MURDoX.Services.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static string ToTimestamp(this DateTimeOffset dto, TimestampFormat format = TimestampFormat.Relative)
            => $"<t:{dto.ToUniversalTime()}:{(char)format}>";
    }
}
