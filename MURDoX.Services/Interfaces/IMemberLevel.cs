using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Interfaces
{
    public interface IMemberLevel
    {
        Task<bool> LevelUp(DiscordUser user);
    }
}
