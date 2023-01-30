using DSharpPlus;
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
            query.Replace(" ", "-");
            var memeUrl = MemeHelper.GetMemeUrl(query);
            var embedBuilder = new EmbedBuilderHelper();

            await ctx.Channel.TriggerTypingAsync();
            var memeEmbed = new EmbedBuilderHelper();
            if (!memeUrl.Equals("https:"))
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
                //var testUrl = "https://i.imgflip.com/4/345v97.jpg";
               
                //memeEmbed.WithImageUrl(memeUrl).Build();

                //await Task.Delay(500);

                await ctx.Channel.SendMessageAsync(embed: memeEmbed.Build(embed));
                //await ctx.Channel.SendMessageAsync("``Apparently`` there is a bug in ``Discord``.....so until further notice [fuck the meme generator]!");
            }
            else
            {
                var notFoundEmbed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = "no meme was found, please try again.",
                };
                await ctx.Channel.SendMessageAsync(embed: memeEmbed.Build(notFoundEmbed));
            }
               
        }
    }
}
