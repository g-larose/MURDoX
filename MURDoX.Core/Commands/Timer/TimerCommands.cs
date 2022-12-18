using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MURDoX.Data.Assets;
using MURDoX.Services.Helpers;
using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using System.Diagnostics;
using MURDoX.Core.Extensions;

namespace MURDoX.Core.Commands.Timer
{
    public class TimerCommands : BaseCommandModule
    {
        private readonly IDataService _dataService;
        private Stopwatch _timer { get; set; }

        private bool isRunning;

        public TimerCommands(IDataService dataService)
        {
            _timer = new Stopwatch();
            _dataService = dataService;
            
        }

        #region START COMMAND
        [Command("start")]
        [Description("starts a timer event")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task Start(CommandContext ctx, [RemainingText] string args)
        {
            var bot = ctx.Client.CurrentUser;
            Random rnd = new Random();

            if (args.Length < 1)
            {
                  await ctx.Channel.SendMessageAsync("wrong number of arguments, command failure.");
                return;
            }
              

            FactsJson? facts = _dataService.LoadFactsJson();
            var factsList = new List<string>();
            foreach (var f in facts.Facts!)
            {
                factsList.Add(f);
            }
          
            var _builder = new EmbedBuilderHelper();
            var messageAuthor = ctx.Message.Author;
            var botAvatar = ctx.Client.CurrentUser.AvatarUrl;
            var botName = ctx.Client.CurrentUser.Username;
            _timer.Start();
            isRunning = true;

            while (isRunning)
            {
                var minutes = _timer.ElapsedMilliseconds.ToMinutes();
                var hours = _timer.ElapsedMilliseconds.ToHours();
                int index;
                string fact;
            
                if (factsList.Count < 1)
                {
                    Embed embed = new Embed()
                    {
                        Desc = "Fact List is Empty!"
                    };
                    await ctx.Channel.SendMessageAsync(_builder.Build(embed));
                    isRunning = false;
                    return;
                }
                else
                {
                    if (factsList.Count == 1)
                    {
                        
                        _timer.Restart();
                        Embed embed = new()
                        {
                            Author = botName,
                            Color = "orange",
                            Title = "Random Fact of the Day",
                            Desc = factsList[0],
                            Footer = $"{botName}©️ {DateTime.Now.ToLongDateString()}",
                            AuthorAvatar = botAvatar,
                            ImgUrl = null,
                            ThumbnailImgUrl = botAvatar,
                            FooterImgUrl = botAvatar,
                        };

                        await ctx.Channel.SendMessageAsync(_builder.Build(embed));
                        factsList.RemoveAt(0);
                    }
                    else
                    {
                        if (minutes >= 1)
                        {
                            index = rnd.Next(factsList.Count);
                            fact = factsList[index];
                            try
                            {
                                factsList.RemoveAt(index);
                                _timer.Restart();
                                Embed embed = new()
                                {
                                    Author = botName,
                                    Color = "orange",
                                    Title = "Random Fact of the Day",
                                    Desc = fact,
                                    Footer = $"{botName}©️ {DateTime.Now.ToLongDateString()}",
                                    AuthorAvatar = botAvatar,
                                    ImgUrl = null,
                                    ThumbnailImgUrl = botAvatar,
                                    FooterImgUrl = botAvatar,
                                };

                                await ctx.Channel.SendMessageAsync(_builder.Build(embed));
                            }
                            catch (Exception ex)
                            {
                                var e = ex.Message;
                            }

                        }
                    }
                   
                }

            }

        }
        #endregion

        #region STOP COMMAND
        [Command("stop")]
        [Description("stops a timer event")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task Stop(CommandContext ctx, [RemainingText] string args)
        {
            var _builder = new EmbedBuilderHelper();
            var embed = new Embed();
            var botAvatar = ctx.Client.CurrentUser.AvatarUrl;
            var botName = ctx.Client.CurrentUser.Username;
            var messageAuthor = ctx.Message.Author;
            if (isRunning == false)
            {
                embed = new Embed()
                {
                    Desc = $"Message of the Day is not currently Running\r\nstart Message of the Day by envoking command ```[start timer]```",
                    Footer = $"{botName}©️ {DateTime.Now.ToLongDateString()}"
                };
                await ctx.Channel.SendMessageAsync(_builder.Build(embed));
                return;

            }
            embed = new Embed()
            {
                Author = botName,
                Color = "orange",
                Title = "Random Fact of the Day",
                Desc = $"Random Fact of the Day Stopped by ```{messageAuthor.Username}```",
                Footer = $"{botName}©️ {DateTime.Now.ToLongDateString()}",
                AuthorAvatar = botAvatar,
                ImgUrl = null,
                ThumbnailImgUrl = botAvatar,
                FooterImgUrl = botAvatar,
            };
            isRunning = false;
            await ctx.Channel.SendMessageAsync(_builder.Build(embed));
        } 
        #endregion

    }
}
