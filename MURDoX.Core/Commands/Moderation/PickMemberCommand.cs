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
            var guild = ctx.Guild;
            IEnumerable<DiscordMember> mems = await guild.GetAllMembersAsync();
           
            int seed = DateTime.UtcNow.Millisecond + DateTime.UtcNow.Second;
            var random = new Random(seed);
            var index = random.Next(mems.Count());

            DiscordMember selectedUser;

            do //loop until the selectedUser is not a bot
            {
                selectedUser = mems.ElementAt(index);
            }
            while (selectedUser.IsBot == true);

           var msg =  await ctx.Channel.SendMessageAsync($"and the winner is... ``{selectedUser.Username}``");
           await Task.Delay(2000);
           await msg.ModifyAsync($"{selectedUser.Username} please dm a staff member for your prize!");
        }
    }
}
