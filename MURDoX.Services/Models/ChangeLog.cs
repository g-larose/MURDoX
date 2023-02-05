using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
{
    public class ChangeLog
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTimeOffset Created_Timestamp { get; set; }
        public ChangeLogStatus Status { get; set; }
    }
}
