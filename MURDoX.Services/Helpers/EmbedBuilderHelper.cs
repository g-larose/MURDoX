using DSharpPlus;
using DSharpPlus.Entities;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Helpers
{
    public class EmbedBuilderHelper
    {
        public DiscordEmbed Build(Embed embed)
        {
            var _embed = new DiscordEmbedBuilder()
                .WithAuthor(embed.Author, "", embed.AuthorAvatar)
                .WithColor(EmbedColors.GetColor(embed.Color!, DiscordColor.DarkGray))
                .WithTitle(embed.Title)
                .WithDescription(embed.Desc)
                .WithUrl(embed.LinkUrl)
                .WithImageUrl(embed.ImgUrl)
                .WithThumbnail(embed.ThumbnailImgUrl)
                .WithTimestamp(embed.TimeStamp)
                .WithFooter(embed.Footer, embed.FooterImgUrl);

            if (embed.Fields != null)
            {
                foreach (var field in embed.Fields)
                {
                    if (field != null)
                        _embed.AddField(field.Name, field.Value, field.Inline);
                }
            }

            _embed.Build();

            return _embed;
        }

       
    }
}
