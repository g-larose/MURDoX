using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MURDoX.Services.Helpers;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Core.Commands.Memes
{
    public class MemeCommands : BaseCommandModule
    {
        [Command("meme")]
        [Description("sends a random meme to the channel.")]
        public async Task LoadRandomMeme(CommandContext ctx, [RemainingText] string query)
        {
            var bot = ctx.Client.CurrentUser;
            var memeUrl = MemeHelper.GetMemeUrl(query);
            var embedBuilder = new EmbedBuilderHelper();

            await ctx.Channel.TriggerTypingAsync();

            if (memeUrl is not null)
            {
                var embed = new Embed()
                {
                    Title = $"{query} meme",
                    Author = $"{bot.Username} ",
                    Desc = $"",
                    Footer = $"{bot.Username}©️",
                    AuthorAvatar = bot.AvatarUrl,
                    LinkUrl = "",
                    ImgUrl = memeUrl,
                    TimeStamp = DateTimeOffset.Now,
                    FooterImgUrl = bot.AvatarUrl,
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                };


                await ctx.Channel.SendMessageAsync(embedBuilder.Build(embed));
            }
        }
    }
}
