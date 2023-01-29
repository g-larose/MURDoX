using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MURDoX.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Games._8Ball
{
    public class _8BallCommands : BaseCommandModule
    {
        [Command("8ball")]
        [Description("get a random 8 ball prediction")]
        public async Task _8Ball(CommandContext ctx, [RemainingText] string query)
        {
            //TODO: error handling
            var saying = GameHelper.Generate_8Ball_Saying();
            await ctx.Channel.SendMessageAsync($"you asked: ``{query}`` Response: ``{saying}``");
        }
        

    }
}
