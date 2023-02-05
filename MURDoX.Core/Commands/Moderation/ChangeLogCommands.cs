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
        [RequireRoles(RoleCheckMode.MatchNames, new string[] { "mod", "Admin", "Administrator", "Lead Developer" })]
        public async Task ManageChangeLog(CommandContext ctx, [RemainingText] string args)
        {
            /*
            example change log - !changelog add, name, content (this is the description of the changelog), ststus.
            the id and timestamp will be auto generated when the command is run.
            0 = command ie 'add', 'edit', 'list' or 'remove'
            1 = name - the name of the changelog. until I comeup with a better idea
            2 = content - the changelog data
            3 = status - the status
            */
            var bot = ctx.Client.CurrentUser;
            var changeLogHelper = new ChangeLogHelper();
            var argDetails = args.Split(',');
            var embedBuilder = new EmbedBuilderHelper();
            var embed = new Embed();
            var changeLog = new ChangeLog();

            if (argDetails.Length != 4) //if the wrong number of arguments are sent , the changelog cannot be built or saved. 
            {
                embed = new()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"incorrect changelog arguments... command canceled!",
                };
                await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                return;
            }
            switch (argDetails[0]) //handle the command parameter.
            {
                case "add":
                    changeLog = new()
                    {
                        Id = Guid.NewGuid(),
                        Name = argDetails[1],
                        Content = argDetails[2],
                        Created_Timestamp = DateTimeOffset.UtcNow,
                    };

                    var result = await changeLogHelper.SaveChangelogToFileAsync(changeLog);

                    if (result == 0)
                    {
                        embed = new()
                        {
                            Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                            Desc = $"change log {changeLog.Id} has been created",
                            Footer = "MURDoX",
                            TimeStamp= DateTimeOffset.UtcNow,
                            FooterImgUrl = bot.AvatarUrl,
                        };
                        await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                        break;
                    }
                    embed = new()
                    {
                        Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                        Desc = $"changelog {changeLog.Id} could not be created, see server-log for more details",
                    };
                    await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));

                    break;
                case "list":

                    break;
                case "remove":

                    break;
                case "edit":

                    break;
            }
           

            




        }

    }
}
