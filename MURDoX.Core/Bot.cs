using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using MURDoX.Data;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using MURDoX.Services.Services;
using NodaTime;
using NodaTime.TimeZones;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MURDoX.Core
{
    public class Bot
    {
        DiscordClient? client { get; set; }
        public CommandsNextExtension? Commands { get; set; }
        public SlashCommandsExtension? SlashCommands { get; set; }
        public static Bot Instance { get; } = new Bot();
        public static InteractivityExtension? Interactivity { get; }
        private ILoggerService _logger { get; set; }
        private List<Log> chatLogs { get; set; }

        private Queue<string> messageQue = new Queue<string>();
        private Dictionary<ulong, int> xpList = new Dictionary<ulong, int>();

        private List<(ulong, string)> guildIdList = new List<ValueTuple<ulong, string>>();
        public Bot()
        {
            _logger = new LoggerService();
            chatLogs = new List<Log>();
           // guildIdList.Add(new ValueTuple<ulong, string>(795412870438453318, "Suncoast Software Community"));
           // guildIdList.Add(new ValueTuple<ulong, string>(764184337620140062, "Bot Playground"));
            guildIdList.Add(new ValueTuple<ulong, string>(995636157595521054, "DroppsDev"));
        }


        #region RUNASYNC
        public async Task RunAsync()
        {
            var dataService = new DataService();
            var configJson = dataService.GetApplicationConfig();

            var clientConfig = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                AlwaysCacheMembers = true,  
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,

            };

            //initialize the client
            client = new DiscordClient(clientConfig);
            client.Ready += Client_Ready;

            //set up client interactivity configuration
            var iteractivityConfig = new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(1),       
                PollBehaviour = PollBehaviour.KeepEmojis,
                PaginationEmojis = new PaginationEmojis(),
                PaginationBehaviour = PaginationBehaviour.WrapAround,
                PaginationDeletion = PaginationDeletion.KeepEmojis
            };

            client.UseInteractivity(iteractivityConfig);

            //configure the services
            var services = new ServiceCollection()
                .AddSingleton<AppDbContextFactory>()
                .AddTransient<IUserService, UserService>()
                .AddSingleton<IDataService, DataService>()
                .AddSingleton<ITimerService, TimerService>()
                .AddScoped<IXmlDataService, XmlDataService>()
                .AddSingleton<ILoggerService, LoggerService>()
                .AddSingleton<IDateTimeZoneSource>(TzdbDateTimeZoneSource.Default)
                .AddSingleton<IDateTimeZoneProvider, DateTimeZoneCache>()
                .AddTransient<TimerServiceHelper>()
                .BuildServiceProvider();

            //set up commands configuration
            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix![0], configJson.Prefix![1] },
                EnableDms = true,
                EnableMentionPrefix = true,
                Services = services
            };

            //register commands and slash commands
            this.Commands = this.client.UseCommandsNext(commandsConfig);
            this.SlashCommands = this.client.UseSlashCommands();
            RegisterCommands();
            RegisterSlashCommands();

            client.MessageCreated += Client_MessageCreated;
            client.MessageDeleted += Client_MessageDeleted;
            client.ClientErrored += Client_ClientErrored;
            client.GuildMemberAdded += Client_GuildMemberAdded;
            client.GuildMemberRemoved += Client_GuildMemberRemoved;

            //Migrate the Database
            //Startup startUp = new Startup();
            //startUp.ConfigureDb(new AppDbContext());

            //connect client to gateway
            await client.ConnectAsync(new DiscordActivity("Everyone", ActivityType.Watching)).ConfigureAwait(false);

            //start the server timer
            ITimerService timerService;
            timerService = new TimerService();
            timerService.Start();

            //get guild members
            var userService = new UserService(new AppDbContextFactory());
            foreach (var guild in guildIdList)
            {
                await userService.GetServerMembersOnStartupAsync(await client.GetGuildAsync(guild.Item1));
            }

            //wait - this keeps the console running
            await Task.Delay(-1).ConfigureAwait(false);
        }

        #region MESSAGE DELETED
        private async Task Client_MessageDeleted(DiscordClient sender, MessageDeleteEventArgs e)
        {
            var mentions = e.Message.MentionedUsers.ToList();
            if (mentions.Count > 0)
            {
                var guild = e.Guild;
                var channel = e.Message.Channel;
                var pingAuthor = e.Message.Author;
                var embedBuilder = new EmbedBuilderHelper();
                var fields = new EmbedField[2];
                fields[0] = new EmbedField { Name = "Author", Value = pingAuthor.Mention, Inline = true };
                fields[1] = new EmbedField { Name = "Message", Value = e.Message.Content, Inline = true };
                var embed = new Embed()
                {
                    Title = "MURDoX caught a ghost ping!",
                    Color = "red",
                    Fields = fields,
                    ThumbnailImgUrl = "https://i.imgur.com/C575c6Q.png",
                    Footer = "MURDoX ",
                    TimeStamp = DateTime.UtcNow,
                };

                switch (guild.Name)
                {
                    case "DroppsDev":
                        var logChannelId = (ulong)1004370669649281034;
                        var logChannel = e.Guild.GetChannel(logChannelId);
                        await logChannel.SendMessageAsync(embed: embedBuilder.Build(embed));
                        break;
                    case "Bot Playground":
                        logChannelId = (ulong)888659367824601160;
                        logChannel = e.Guild.GetChannel(logChannelId);
                        await logChannel.SendMessageAsync(embed: embedBuilder.Build(embed));
                        break;
                    default:

                        break;
                }
                await channel.SendMessageAsync(embed: embedBuilder.Build(embed));
            }
            else
            {
                var msgService = new DiscordMessageService();
                await msgService.HandleDeletedMessage(e.Channel, e.Message);
            }


        }
        #endregion

        #region GUILD MEMBER REMOVED
        private async Task Client_GuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs e)
        {
            var channelId = (ulong)795418412536168448;
            var channel = await sender.GetChannelAsync(channelId);
            await channel.SendMessageAsync($"{e.Member.Username} has left the server");

        }
        #endregion

        #region GUID MEMBER ADDED
        private async Task Client_GuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
        {
            var user = e.Member;
            var welcomeMessage = WelcomerHelper.GenerateWelcomeMessage();
            welcomeMessage = welcomeMessage.Replace("User", user.Username);
            await e.Member.SendMessageAsync(welcomeMessage);

            if (e.Guild.Name.Equals("DroppsDev"))
            {
                ulong DroppsDevPublicGeneralChannelId = 1026762217372254228;
                var channel = e.Guild.GetChannel(DroppsDevPublicGeneralChannelId);
                await channel.SendMessageAsync(welcomeMessage);
            }
        }
        #endregion


        #region CLIENT ERRORED
        private Task Client_ClientErrored1(DiscordClient sender, ClientErrorEventArgs e)
        {
            var _errorEvent = e.Exception.GetType();
            var _errorName = e.EventName;

            return Task.CompletedTask;


        } 
        #endregion

        #endregion

        #region CLIENT ERRORED
        private Task Client_ClientErrored(DiscordClient sender, DSharpPlus.EventArgs.ClientErrorEventArgs e)
        {
            Task.Run(async () =>
            {

            });

            return Task.CompletedTask;
        }
        #endregion

        #region MESSAGE CREATED
        private Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            Task.Run(async () =>
            {
                var channel = e.Message.Channel;
                var author = e.Message.Author;
                var message = e.Message;

                if (author.IsBot) return;

                LevelHelper.SetXp(author.Username, 1);

                if (message.Content.StartsWith("$"))
                {
                    var client = this.client!;
                    var embedBuilder = new EmbedBuilderHelper();
                    var details = message.Content.Split("$");
                    var tagCommand = details[1];
                    var embed = TagHelper.RequestTag(client, "MURDoX", tagCommand);
                    await e.Message.Channel.SendMessageAsync(embed: embed);

                    return;
                }
                if (message.Content.Contains("thanks"))
                {

                }

                var xp = await LevelHelper.GetXp(author.Username);
                switch (xp)
                {
                    case int n when (n >= 3000 && n <= 15000):
                        LevelHelper.SetRank(author.Username, Rank.REGULAR);
                        break;
                    case int n when (n >= 15001 && n <= 35000):
                        LevelHelper.SetRank(author.Username, Rank.ASSOCIATE);
                        break;
                    case 35001:
                        LevelHelper.SetRank(author.Username, Rank.MASTER);
                        break;
                }
              
                //messageQue.Enqueue(message.Content.ToString());
                //xpList.Add(author.Id, xp);
                //ulong logChannelId = message.ChannelId;


                //_logger.Save(chatLog);

                // var banneddWords = new string[] { "shit", "bitch", "asshole", "fuck", "fucker", "dickhead", "pussy" };
                // var channel = await sender.GetChannelAsync(logChannelId).ConfigureAwait(false);
                // var guildId = e.Guild.Id;

                
               
            });

            return Task.CompletedTask;

        }
        #endregion

        #region CLIENT READY
        private async Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            var bot = sender.CurrentUser.Username;
            Console.ForegroundColor = FromHex("#0570fc");
            Console.WriteLine($"\r\n{bot} connected successfully!\r\n");

            await SendLogMessage($"{bot} has joined the server");
        } 
        #endregion

        private void RegisterCommands() => client.GetCommandsNext().RegisterCommands(Assembly.GetExecutingAssembly());
        private void RegisterSlashCommands() => client.GetSlashCommands().RegisterCommands(Assembly.GetExecutingAssembly());

        #region SEND LOG MESSAGE

        private async Task SendLogMessage(string message)
        {
            //general channel ID 764184337620140065
            var logIds = new ulong[] { 888659367824601160 }; // add channel id's here for every channel to send logs to.

            foreach (var id in logIds)
            {
                var logChannel = await client!.GetChannelAsync(id).ConfigureAwait(false);
                await logChannel.SendMessageAsync(message);
            }

        }

        #endregion

        public static ConsoleColor FromHex(string hex)
        {
            int argb = Int32.Parse(hex.Replace("#", ""), NumberStyles.HexNumber);
            Color c = Color.FromArgb(argb);

            int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0; // Bright bit
            index |= (c.R > 64) ? 4 : 0; // Red bit
            index |= (c.G > 64) ? 2 : 0; // Green bit
            index |= (c.B > 64) ? 1 : 0; // Blue bit

            return (System.ConsoleColor)index;
        }
    }
}
