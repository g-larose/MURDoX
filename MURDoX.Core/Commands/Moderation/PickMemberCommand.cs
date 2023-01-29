using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Moderation
{
    public class PickMemberCommand : BaseCommandModule
    {
        [Command("pickwinner")]
        [Description("pick a random server member, use to select a winner for give aways")]
        public async Task PickRandomMember(CommandContext ctx)
        {
            List<DiscordMember> users = ctx.Guild.Members.Values.Where(u => !u.IsBot).ToList();

            int seed = DateTime.UtcNow.Millisecond + DateTime.UtcNow.Second;
            var random = new Random(seed);

            DiscordMember selectedUser = users[random.Next(users.Count)];

            await ctx.Channel.SendMessageAsync($"and the winner is... ``{selectedUser.Username}``");
        }
    }
}
