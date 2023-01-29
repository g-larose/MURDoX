using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Tag
{
    public class TagCommands : BaseCommandModule
    {
        private readonly IDataService _dataService;
        private readonly AppDbContextFactory _dbFactory;
        public TagCommands(IDataService dataService, AppDbContextFactory dbFactory)
        {
            _dataService = dataService;
            _dbFactory = dbFactory;
            
        }

        #region ADD TAG COMMAND
        [Command("addtag")]
        [Description("add a tag")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task AddTag(CommandContext ctx, [RemainingText] string args)
        {
            //ex tag command
            // !addtag category, name, description, content
        }
        #endregion


        #region GET ALL TAGS
        [Command("listtags")]
        [Description("returns a list of tags")]
        public async Task ListTags(CommandContext ctx)
        {
            //TODO: this should be a paginated response embed
            var bot = ctx.Client.CurrentUser;
            var db = _dbFactory.CreateDbContext();
            var tags = db.Tags!.OrderByDescending(x => x.CreateAt).Take(5).ToList();

            if (tags.Count == 0) 
            {
                await ctx.Channel.SendMessageAsync("there are no **tags** to list");
            }
            else
            {
                var tagBuilder = new StringBuilder();
                var count = 1;
                foreach (var tag in tags)
                {
                    tagBuilder.Append($"**{count}.** {tag.TagName}- {tag.Content}\r\n");
                    count++;
                }

                var embedBuilder = new EmbedBuilderHelper();
                var embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Title = "TAGS",
                    Author = ctx.Member.Username,
                    AuthorAvatar = ctx.Member.AvatarUrl,
                    Desc = tagBuilder.ToString(),
                    Footer = "MURDoX",
                    FooterImgUrl = bot.AvatarUrl,
                    TimeStamp = DateTime.UtcNow,
                };

                await ctx.TriggerTypingAsync();
                await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            }
            
        }
        #endregion
    }
}
