using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Moderation
{
    public class ChangeLogCommands : BaseCommandModule
    {
        [Command("changelog")]
        [Description("add a new changelog, only roles of 'mod' or higher can run this command")]
        [RequireRoles(RoleCheckMode.MatchNames, new string[] { "mod", "Administrator" })]
        public async Task ManageChangeLog(CommandContext ctx, [RemainingText] string args)
        {
            //ex change log - !changelog add, name, content (this is the description of the changelog), ststus.
            //the id and timestamp will be auto generated when the command is run.
            //0 = command argument
            //1 = name
            //2 = content
            //3 = status
            var argDetails = args.Split(',');
            var changeLog = new ChangeLog()
            {
                Id = Guid.NewGuid(),
                Name = argDetails[1],
                Content = argDetails[2],
                Created_Timestamp = DateTimeOffset.UtcNow,
            };

            var result = ChangeLogHelper.SaveChangelogToFile(changeLog);
            if (result == 0)
            {
                //we have success , send embed to channel.
            }
            else
            {
                //we have failure, send embed to channel.
            }
        }

    }
}
