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
            var embedBuilder = new EmbedBuilderHelper();
            Embed embed;
            var tagDetails = args.Split(',');
            if (tagDetails.Length < 4)
            {
                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = "Wrong number of arguments. example Tag **[category, name, description, content]**"
                };
                await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            }
            else
            {
                var tag = new Services.Models.Tag()
                {
                    Category = tagDetails[0].Trim(),
                    TagName = tagDetails[1].Trim(),
                    TagDesc = tagDetails[2].Trim(),
                    Content = tagDetails[3].Trim(),
                    CreatedBy = ctx.Message.Author.Username,
                    CreateAt = DateTime.UtcNow,
                };

                var db = _dbFactory.CreateDbContext();
                db.Add(tag);
                await db.SaveChangesAsync();

                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"tag **{tag.TagName}** created!"
                };
                await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            }
            

        }
        #endregion


        #region GET ALL TAGS
        [Command("listtags")]
        [Description("returns a list of tags")]
        public async Task ListTags(CommandContext ctx)
        {
            //TODO: this should be a paginated response embed
            await ctx.TriggerTypingAsync();
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
                    Title = "Top 5 TAGS",
                    Author = ctx.Member.Username,
                    AuthorAvatar = ctx.Member.AvatarUrl,
                    Desc = tagBuilder.ToString(),
                    Footer = "MURDoX",
                    FooterImgUrl = bot.AvatarUrl,
                    TimeStamp = DateTime.UtcNow,
                };

               
                await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            }
            
        }
        #endregion

        [Command("tag")]
        [Description("gets a single tag")]
        public async Task Tag(CommandContext ctx, [RemainingText] string tagName)
        {
            var tagHelper = new TagHelper(_dbFactory);
            var EmbedBuilder = new EmbedBuilderHelper();
            Embed embed;
            var tag = tagHelper.RequestSingleTagFromDb(tagName);

            if (tag == null)
            {
                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = "no tag found."
                };

                await ctx.Channel.SendMessageAsync(embed: EmbedBuilder.Build(embed));
            }
            else
            {
                var fields = new EmbedField[]
                {
                    new() { Name = "Category", Value = $"{tag.Category}", Inline = true },
                    new() { Name = "Tag Name", Value = $"{tag.TagName}", Inline = true },
                    new() { Name = "Description", Value = $"{tag.TagDesc}", Inline = true },
                    new() { Name = "Content", Value = $"{tag.Content}", Inline = true },
                    new() { Name = "Create By", Value = $"{tag.CreatedBy}", Inline = true },
                    new() { Name = "Created At", Value = $"{tag.CreateAt}", Inline = true },
                };

                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Title = $"{tag.TagName}",
                    Author = ctx.Message.Author.Username,
                    AuthorAvatar = ctx.Message.Author.AvatarUrl,
                    Fields = fields,
                    Footer = "MURDoX ",
                    TimeStamp= DateTime.UtcNow,
                    FooterImgUrl = ctx.Client.CurrentUser.AvatarUrl,

                };

                await ctx.Channel.SendMessageAsync(embed: EmbedBuilder.Build(embed));
            }
        }
    }
}
