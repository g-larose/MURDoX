using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Suggestions
{
    public class SuggestionCommands : BaseCommandModule
    {
        private readonly AppDbContextFactory _dbFactory;
        //channel id 1065545582615216179

        public SuggestionCommands(AppDbContextFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        [Command("suggestion")]
        [Description("add a server-suggestion for the admin to discuss")]
        public async Task Suggestion(CommandContext ctx, [RemainingText] string args)   
        {
            var author = ctx.Message.Author;
            var details = args.Split(',');
            var bot = ctx.Client.CurrentUser;

            if (details.Length > 0 || details.Length < 3)
            {
                var title = details[0];
                var content = details[1];
                var suggestion = new Suggestion()
                {
                    AuthorId = author.Id,
                    Name = title,
                    Discription = content,
                    Created = DateTime.UtcNow,
                };

                var db = _dbFactory.CreateDbContext();
                db.Suggestions!.Add(suggestion);
                await db.SaveChangesAsync();

                //var interactivity = ctx.Client.GetInteractivity();
                var embedBuilder = new EmbedBuilderHelper();
                var embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"``{suggestion.Name}`` created : by ``{author.Username}`` : at {suggestion.Created}",
                    Footer = $"{bot.Username} {DateTimeOffset.Now.UtcDateTime}"
                };

                var droppsDevChannel = ctx.Guild.GetChannel(1065545582615216179);
                await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));
                await droppsDevChannel.SendMessageAsync($"suggestion [{suggestion.Name}] has been created"); 
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"``{author.Username}`` passed invalid arguments, please try again!");
            }    
        }
    }
}
