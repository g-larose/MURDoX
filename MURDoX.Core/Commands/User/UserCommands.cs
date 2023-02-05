﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using MURDoX.Services.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
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


    }
}
