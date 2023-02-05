using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
{
    public class LogMessage
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Creator { get; set; } = string.Empty;
        public LogType Type { get; set; }
        public DateTimeOffset Created_Timestamp { get; set; }
    }
}
