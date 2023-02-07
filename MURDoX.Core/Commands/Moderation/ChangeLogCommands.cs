using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MURDoX.Services.Extensions;
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

            switch (argDetails[0]) //handle the command parameter.
            {
                #region ADD
                case "add":
                    if (argDetails.Count() != 4) //if the wrong number of arguments are sent , the changelog cannot be built or saved. 
                    {
                        embed = new()
                        {
                            Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                            Desc = $"incorrect changelog arguments... command canceled!",
                        };
                        await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                        return;
                    }
                    changeLog = new()
                    {
                        Id = Guid.NewGuid(),
                        Name = argDetails[1],
                        Content = argDetails[2],
                        Status = argDetails[3].Trim().ConvertChangeLogStatusFromString(),
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
                            TimeStamp = DateTimeOffset.UtcNow,
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
                #endregion
                #region List
                case "list":
                    var changeLogs = await changeLogHelper.GetChangeLogListAsync();
                    embedBuilder = new EmbedBuilderHelper();
                    embed = new Embed();
                    var descBuilder = new StringBuilder();
                    foreach (var log in changeLogs)
                    {
                        descBuilder.Append($"change log: {log.Id}: {log.Name}: {log.Content}: {log.Status}: {log.Created_Timestamp}\r\n");
                    }
                    if (changeLogs.Count > 0)
                    {
                        embed = new Embed()
                        {
                            Color = "blurple",
                            Desc = descBuilder.ToString(),   
                        };
                        await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                    }
                    break;
                #endregion
                #region REMOVE
                case "remove":

                    break;
                #endregion
                #region EDIT
                case "edit":

                    break; 
                #endregion
            }
           

            




        }

    }
}
