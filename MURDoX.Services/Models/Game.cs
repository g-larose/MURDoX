using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Models
{
    public class Game
    {
        public Guid GameId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public List<(string Player, int Wins)>? Players { get; set; }
        public bool IsRunning { get; set; }
    }
}
