using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using MURDoX.Services.Services;
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

        public Bot()
        {
            _logger = new LoggerService();
            chatLogs = new List<Log>();
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
                .AddTransient<AppDbContextFactory>()
                .AddTransient<IUserService, UserService>()
                .AddSingleton<IDataService, DataService>()
                .AddSingleton<ITimerService, TimerService>()
                .AddSingleton<ILoggerService, LoggerService>()
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
            client.ClientErrored += Client_ClientErrored;
            client.GuildMemberAdded += Client_GuildMemberAdded;
            client.GuildMemberRemoved += Client_GuildMemberRemoved;
            

            //connect client to gateway
            await client.ConnectAsync(new DiscordActivity("Everyone", ActivityType.Watching)).ConfigureAwait(false);

            //start the server timer
            ITimerService timerService;
            timerService = new TimerService();
            timerService.Start();

            //var messages = await dataService.LoadJsonAsync<WelcomeMessageJson>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Welcomer", "welcome_messages.json"));
            //wait - this keeps the console running
            await Task.Delay(-1).ConfigureAwait(false);
        }

        private async Task Client_GuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs e)
        {
            var channelId = (ulong)795418412536168448;
            var channel = await sender.GetChannelAsync(channelId);
            await channel.SendMessageAsync($"{e.Member.Mention} has left the server");

        }

        private async Task Client_GuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
        {
           await e.Member.SendMessageAsync("welcome to Suncoast Software Community, please read #rules");
        }

        private Task Client_ClientErrored1(DiscordClient sender, ClientErrorEventArgs e)
        {
            var _errorEvent = e.Exception.GetType();
            var _errorName = e.EventName;
            return Task.CompletedTask;
            
            
        }

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
                var author = e.Message.Author;
                var message = e.Message;
                ulong logChannelId = message.ChannelId;
                var chatLog = new Log()
                {
                    LogId = logChannelId,
                    AuthorId = author.Id,
                    Title = "ChatMessage",
                    Desc = "Chat Message",
                    Message = message.Content,
                    Type = LogType.MESSAGE,
                    Created = DateTime.Now.ToUniversalTime()
                };
                chatLogs.Add(chatLog);
                //_logger.Save(chatLog);

                // var banneddWords = new string[] { "shit", "bitch", "asshole", "fuck", "fucker", "dickhead", "pussy" };
                //var channel = await sender.GetChannelAsync(logChannelId).ConfigureAwait(false);
                // var guildId = e.Guild.Id;

                if (author.IsBot) return;//if message is from the bot, ignore it!

                if (e.Message.Content.StartsWith("$"))
                {
                    await e.Message.Channel.SendMessageAsync("the $ is a reserved char command for mods");
                   
                    return;
                }
                if (e.Message.Content.StartsWith("!warn"))
                {
                    var guildId = e.Guild.Id;
                    var guild = await sender.GetGuildAsync(guildId);
                    var user = await guild.GetMemberAsync(e.Message.Author.Id);
                    var roles = user.Roles;
                    var canExecute = false;
                    foreach (var role in roles)
                    {
                        if (role.ToString() == "mod")
                            canExecute = true;
                    }
                    if (canExecute == false)
                    {
                        await e.Message.DeleteAsync();
                        await e.Message.Channel.SendMessageAsync($"**{user.Username}** you dont have permission to execute this command!");
                    }
                       
                     
                }
                //await e.Channel.SendMessageAsync($"messages logged: {chatLogs.Count.ToString()}");
            });

            return Task.CompletedTask;

        }
        #endregion

        #region CLIENT READY
        private async Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            var bot = sender.CurrentUser.Username;
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
    }
}
