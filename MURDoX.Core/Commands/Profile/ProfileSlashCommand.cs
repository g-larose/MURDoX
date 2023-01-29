using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Profile
{
    public class ProfileSlashCommand : ApplicationCommandModule
    {
        [SlashCommand("profile", "gets a discord members profile stats")]
        public async Task Profile(InteractionContext ctx, [Option("user", "username")] string member)
        {
            var dbFactory = new AppDbContextFactory();
            var db = dbFactory.CreateDbContext();
            
            var bot = ctx.Client.CurrentUser;
            var discordUsers = await ctx.Guild.GetAllMembersAsync();
            var userIdStr = member.Replace("@", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty);
            var userId = ulong.Parse(userIdStr);
            var user = discordUsers.Where(x => x.Id == userId).FirstOrDefault();
            var dbUser = db.Users!.Where(x => x.Username == user.Username).FirstOrDefault();

            var roles = user.Roles;
            var rolesBuilder = new StringBuilder();
            var descBuilder = new StringBuilder();

            foreach ( var role in roles)
            {
                if (role == roles.Last())
                    rolesBuilder.Append(role.Name);
                else
                    rolesBuilder.Append(role.Name + ',');
            }

            var interactionBuilder = new DiscordInteractionResponseBuilder();
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, builder: interactionBuilder.WithContent("MURDoX is thinking."));
            await Task.Delay(2000);
            var followUpBuilder = new DiscordFollowupMessageBuilder();

            EmbedField[] fields;
            var embedBuilder = new EmbedBuilderHelper();

            if (dbUser != null)
            {
                //fields = new EmbedField[9];
                //fields[0] = new EmbedField { Name = "Display Name", Value = "```cs\n" + $"{user.DisplayName.PadLeft(12, ' ')} ```", Inline = true };
                //fields[1] = new EmbedField { Name = "Id", Value = "```cs\n" + $"{user.Id.ToString().PadLeft(12, ' ')} ```", Inline = true };
                //fields[2] = new EmbedField { Name = "Created Acct", Value = "```cs\n" + $"{user.CreationTimestamp.ToLocalTime().ToString().PadLeft(12, ' ')} ```", Inline = true };
                //fields[3] = new EmbedField { Name = "Joined", Value = "```cs\n" + $"{user.JoinedAt.ToString().PadLeft(12, ' ')} ```", Inline = true };
                //fields[4] = new EmbedField { Name = "Guild", Value = "```cs\n" + $"{user.Guild.Name.PadLeft(12, ' ')} ```", Inline = true };
                //fields[5] = new EmbedField { Name = "Roles", Value = "```cs\n" + $"{rolesBuilder.ToString().PadLeft(12, ' ')} ```", Inline = true };
                //fields[6] = new EmbedField { Name = "Thanks", Value = "```cs\n" + $"{dbUser.Thanks.ToString().PadLeft(0, ' ')} ```", Inline = true };
                //fields[7] = new EmbedField { Name = "XP", Value = "```cs\n" + $"{dbUser.XP.ToString().PadLeft(2, ' ')} ```", Inline = true };
                //fields[8] = new EmbedField { Name = "Rank", Value = "```cs\n" + $"{dbUser.Rank.ToString().PadLeft(2, ' ')} ```", Inline = true };
                descBuilder.Append($"**USER INFO**\r\nID: {user.Id}\r\nHandle: {user.Username}\r\nCreated Acct: {user.CreationTimestamp.ToLocalTime().ToString()}\r\n");
                descBuilder.Append($"Joined: {user.JoinedAt}\r\nRoles: {rolesBuilder.ToString()}\r\nThanks: {dbUser.Thanks}\r\n");
                descBuilder.Append($"XP: {dbUser.XP}\r\nRank: {dbUser.Rank}\r\n\r\n");
                descBuilder.Append($"**GUILD INFO**\r\nGuild: {user.Guild.Name}\r\n");
            }
            else
            {
                //fields = new EmbedField[6];
                //fields[0] = new EmbedField { Name = "Display Name", Value = "```cs\n" + $"{user.DisplayName.PadLeft(12, ' ')} ```", Inline = true };
                //fields[1] = new EmbedField { Name = "Id", Value = "```cs\n" + $"{user.Id.ToString().PadLeft(12, ' ')} ```", Inline = true };
                //fields[2] = new EmbedField { Name = "Created Acct", Value = "```cs\n" + $"{user.CreationTimestamp.ToLocalTime().ToString().PadLeft(12, ' ')} ```", Inline = true };
                //fields[3] = new EmbedField { Name = "Joined", Value = "```cs\n" + $"{user.JoinedAt.ToString().PadLeft(12, ' ')} ```", Inline = true };
                //fields[4] = new EmbedField { Name = "Guild", Value = "```cs\n" + $"{user.Guild.Name.PadLeft(12, ' ')} ```", Inline = true };
                //fields[5] = new EmbedField { Name = "Roles", Value = "```cs\n" + $"{rolesBuilder.ToString().PadLeft(12, ' ')} ```", Inline = true };
                descBuilder.Append($"USER INFO\r\nID: {user.Id}\r\nHandle: {user.Username}\r\nCreated: {user.CreationTimestamp.ToLocalTime().ToString()}\r\n");
                descBuilder.Append($"Joined: {user.JoinedAt}\r\nGuild: {user.Guild.Name}\r\nRoles: {rolesBuilder.ToString()}\r\n\r\n");
                descBuilder.Append($"**GUILD INFO**\r\nGuild: {user.Guild.Name}\r\n");
                
            }

           
            var embed = new Embed()
            {
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                //Title = user.DisplayName,
                Desc = descBuilder.ToString(),
                AuthorAvatar = user.AvatarUrl,
                ThumbnailImgUrl = user.AvatarUrl,
                Author = user.DisplayName,
                Fields = null,
                Footer = $"MURDoX ",
                FooterImgUrl = bot.AvatarUrl,
                TimeStamp = DateTime.UtcNow,

            };

            await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            await ctx.FollowUpAsync(builder: followUpBuilder.WithContent($"MURDoX gathered profile info for ``{user.DisplayName}``"));
        }
    }
}
