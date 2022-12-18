using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public Guid PollGuid { get; set; }
        public string PollTitle { get; set; } = string.Empty;
        public string PollQuestion { get; set; } = string.Empty;
        public string PollDescription { get; set; } = string.Empty;
        public ulong AuthorId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
