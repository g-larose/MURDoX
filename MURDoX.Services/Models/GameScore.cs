using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
{
    public class GameScore
    {
        [Key]
        public int Id { get; set; }
        public int Score { get; set; }
        public int ServerMemberId { get; set; }
        public ServerMember? ServerMember { get; set; }
        public string GameName { get; set; } = string.Empty;
    }
}
