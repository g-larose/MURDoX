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

        }
        #endregion

        #region REQUEST TAG COMMAND
        [Command("tag")]
        [Description("sends a tag to the channel")]
        public async Task RequestTag(CommandContext ctx, [RemainingText] string args)
        {
           
            var embedBuilder = TagHelper.RequestTag(ctx, args);
            await ctx.Channel.SendMessageAsync(embedBuilder);
            
        }
        #endregion

        #region GET ALL TAGS
        [Command("tags")]
        [Description("returns a list of tags")]
        public async Task GetTags(CommandContext ctx)
        {
            var db = _dbFactory.CreateDbContext();
            var tags = db.Tags.ToList();
            var tagBuilder = new StringBuilder();
            

        }
        #endregion
    }
}
