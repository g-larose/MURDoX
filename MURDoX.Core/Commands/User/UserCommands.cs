using BenchmarkDotNet.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using MURDoX.Services.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.User
{
    public class UserCommands : BaseCommandModule
    {
        private readonly AppDbContextFactory _dbFactory;

        public UserCommands(AppDbContextFactory dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        #region WHOIS

        [Command("whois")]
        [Description("get's a mentioned member's info")]
        public async Task Whois(CommandContext ctx, [RemainingText] string args)
        {
           
            await ctx.Channel.TriggerTypingAsync();
            var db = _dbFactory.CreateDbContext();
            var bot = ctx.Client.CurrentUser;
            args = args.Replace("@", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty);
            var id = ulong.TryParse(args, out ulong memId);
            var serverMember = await ctx.Guild.GetMemberAsync(memId);
            var user = db.Users.Where(x => x.Username == serverMember.Username).FirstOrDefault();
            var userRoles = new StringBuilder();

            if ((serverMember as DiscordMember).Roles is not null)
            {
               var roles = (serverMember as DiscordMember).Roles;
               var roleList = new List<string>();
                for (int i = 0; i < roles.Count(); i++)
                {
                    if (i == roles.Count() - 1)
                        roleList.Add(roles.ElementAt(i).Name);
                    else
                        roleList.Add(roles.ElementAt(i).Name + ",");
                }
                 userRoles.Append(String.Join(" ", roleList));
            }
            
            var xpField = new EmbedField();
            var thanksField = new EmbedField();
            var warningsField = new EmbedField();

            var nameField = new EmbedField() { Name = "Member", Value = serverMember.Username, Inline = true };
            var rolesField = new EmbedField() { Name = "Roles", Value = $"``{userRoles.ToString()}``", Inline = true };
            var bankField = new EmbedField() { Name = "Bank", Value = "0", Inline = true };

            if (user is not null)
            {
                thanksField = new EmbedField() { Name = "Thanks", Value = user.Thanks.ToString(), Inline = true };
                xpField = new EmbedField() { Name = "XP", Value = user.XP.ToString(), Inline = true };
                warningsField = new EmbedField() { Name = "Warnings", Value = user.Warnings.ToString(), Inline = true };
            }
               
            else
            {
                thanksField = new EmbedField() { Name = "Thanks", Value = "0", Inline = true };
                xpField = new EmbedField() { Name = "XP", Value = "0", Inline = true };
                warningsField = new EmbedField() { Name = "Warnings", Value = "0", Inline = true };
            }
                
            var discordMemberField = new EmbedField() { Name = "Joined", Value = serverMember.JoinedAt.DateTime.ToString(), Inline = true };

            var fields = new EmbedField[] { discordMemberField, nameField, rolesField, xpField, thanksField, warningsField, bankField };
            var embedBuilder = new EmbedBuilderHelper();
            var embed = new Embed()
            {
                Title = $"Whois {serverMember.Username} Requested by: {ctx.Message.Author.Username}",
                Author = $"{bot.Username} ",
                Desc = $"whois info requested for Server Member ``{serverMember.Username}``",
                Footer = $"{bot.Username}©️",
                AuthorAvatar = bot.AvatarUrl,
                LinkUrl = "",
                ThumbnailImgUrl = null,
                TimeStamp = DateTimeOffset.Now,
                FooterImgUrl = bot.AvatarUrl,
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                Fields = fields,
            };

            await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
        }

        #endregion

        #region UPTIME COMMAND

        [Command("uptime")]
        [Description("Returns the total time the bot has been online")]
        public async Task Uptime(CommandContext ctx)
        {
            var botStartDate = TimerService.StartDate;
            var uptime = TimerService.GetBotUpTime();

            var startDateField = new EmbedField { Name = "Started On", Value = $"{botStartDate}", Inline = false };

            //var secondsField = new EmbedField { Name = "Seconds", Value = $"{uptime.Seconds}", Inline = true };
            //var minutesField = new EmbedField { Name = "Minutes", Value = $"{uptime.Minutes}", Inline = true };
            //var hoursField = new EmbedField { Name = "Hours", Value = $"{uptime.Hours}", Inline = true };
            //var daysField = new EmbedField { Name = "Days", Value = $"{uptime.Days}", Inline = true };
            //var weeksField = new EmbedField { Name = "Weeks", Value = $"{uptime.Weeks}", Inline = true };
            //var monthsField = new EmbedField { Name = "Months", Value = $"{uptime.Months}", Inline = true };
            //var yearsField = new EmbedField { Name = "Years", Value = $"{uptime.Years}", Inline = true };

            var messageAuthor = ctx.Message.Author;
            var botAvatar = ctx.Client.CurrentUser.AvatarUrl;
            var botName = ctx.Client.CurrentUser.Username;
            var guildId = ctx.Guild.Id;
            //var fields = new EmbedField[] { startDateField, yearsField, monthsField, weeksField, daysField, hoursField, minutesField, secondsField };
            var embedBuilder = new EmbedBuilderHelper();

            Embed embed = new()
            {
                //Title = $"UPTIME",
                Author = $"{messageAuthor.Username} Requested Uptime!",
                Desc = $"{botName} has been online for **{uptime}**",
                Footer = $"{botName} ©️{DateTime.Now.ToLongDateString()}",
                AuthorAvatar = messageAuthor.GetAvatarUrl(DSharpPlus.ImageFormat.Jpeg),
                ImgUrl = null,
                ThumbnailImgUrl = "https://i.imgur.com/QpDCyCx.png",
                FooterImgUrl = botAvatar,
                Color = "orange",
               // Fields = fields
            };

            var _embed = embedBuilder.Build(embed);

            await ctx.Channel.SendMessageAsync(_embed);
        }

        #endregion

        #region RANK
        /// <summary>
        /// build custom image to show Member rank info
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns>Task</returns>
        [Command("rank")]
        [Description("get's a members server rank")]
        public async Task Rank(CommandContext ctx)
        {
            var imageService = new ImageService(_dbFactory);
            var bot = ctx.Client.CurrentUser;
            var member = (DiscordMember)ctx.Message.Author;
            var image = await imageService.GenerateRankImage(member);

            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Images", "player_rank.png");
            var msgBuilder = new DiscordMessageBuilder();
            var buffer = File.ReadAllBytes(imagePath);
            using FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            using MemoryStream ms = new MemoryStream(buffer, 0, buffer.Length);
            await msgBuilder.WithFile("player_rank.png", ms).SendAsync(ctx.Channel);

        }

        #endregion

        #region ASSIGN ROLE
        /// <summary>
        /// assignable role command for members
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns>Task</returns>
        [Command("getrole")]
        [Description("sets a self assignable user role to command caller.")]
        public async Task GetUserRole(CommandContext ctx)
        {
            var embedBuilder = new EmbedBuilderHelper();
            List<DiscordEmoji> emojiList = new List<DiscordEmoji>();
            try
            {
                emojiList = new List<DiscordEmoji>()
                {
                    DiscordEmoji.FromName(ctx.Client, ":beetle:"),
                    DiscordEmoji.FromName(ctx.Client, ":bookmark:"),
                    DiscordEmoji.FromName(ctx.Client, ":thinksharp:"),
                };
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            var embed = new Embed();
            embed = new Embed()
            {
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                Desc = "react for the assignable role you want to set.",
            };
 
            var roleMessage = await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            foreach (var e in emojiList)
            {
                await roleMessage.CreateReactionAsync(e);
            }

            var interactivity = ctx.Client.GetInteractivity();
            var emojiResult = await interactivity.CollectReactionsAsync(roleMessage, new TimeSpan(0, 0, 10));
            var roleResults = emojiResult.Select(x => x.Emoji.GetDiscordName()).FirstOrDefault();
            ulong role = roleResults switch
            {
                ":thinksharp:" => 1071858138241835219,
                ":beetle:" => 1071869330083545109,
                _ => 1071759676804444211,
            };
            var newRole = ctx.Guild.GetRole(role);
            try
            {
                await ctx.Member!.GrantRoleAsync(newRole); // this is the fucking problem
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            
            embed = new Embed()
            {
                Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                Desc = $"role {newRole.Name} has been granted",
            };
            await roleMessage.ModifyAsync(embed: embedBuilder.Build(embed));
        }
        #endregion

        #region RANDOM NUMBER
     
        [Command("randomnumber")]
        public async Task RandomNumber(CommandContext ctx, [RemainingText] string args)
        {
            var timer = new Stopwatch();
            timer.Start();
            if (args == null) return;
            
            var embedBuilder = new EmbedBuilderHelper();
            var embed = new Embed();
            var details = args.Split(',');

            if (int.TryParse(details[0].ToString(), out int minValue))
            {
                if (int.TryParse(details[1], out int maxValue))
                {
                    Random rnd = new Random();
                    int num = rnd.Next(minValue, maxValue);
                    timer.Stop();
                    embed = new Embed()
                    {
                        Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                        Desc = $"{ctx.Message.Author.Username} picked random number [{num}] from range {minValue} and {maxValue}",
                    };
                    var ns = timer.ElapsedMilliseconds * 1000;
                    await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                    await ctx.Channel.SendMessageAsync($"{timer.ElapsedTicks}ms");
                }
            }

        }
        #endregion
    }
}
