#region

using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MURDoX.Core.Data;

#endregion

// Following line is required for Rider to disable the warning about unused private fields
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace MURDoX.DiscordAccess
{
    public class Startup
    {
        private IConfiguration? Configuration { get; set; }
        private IServiceCollection? Services { get; set; }
        private SlashCommandsExtension? SlashCommands { get; set; }
        private CommandsNextExtension? Commands { get; set; }
        private InteractivityExtension? Interactivity { get; set; }
        private DiscordClient? DiscordClient { get; set; }

        public async Task RunAsync()
        {
            await ConfigureConfiguration();
            await ConfigureServices();
            await ConfigureBot();
            await ConfigureBotEvents();

            if (DiscordClient is null)
            {
                throw new NullReferenceException();
            }
            await DiscordClient.ConnectAsync();
            
            //for testing purposes only.... so we know we connected successfully!
            Console.WriteLine("MURDoX has connected!");
            await Task.Delay(-1);
        }

        private Task ConfigureConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            return Task.CompletedTask;
        }

        private Task ConfigureServices()
        {
            Services = new ServiceCollection()
                .AddPooledDbContextFactory<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(Configuration?.GetConnectionString("DefaultConnection"));
                });
            return Task.CompletedTask;
        }

        private Task ConfigureBot()
        {
            ServiceProvider serviceProvider = Services?.BuildServiceProvider() ?? throw new NullReferenceException();
            DiscordClient = new DiscordClient(new DiscordConfiguration
            {
                Token = Configuration?.GetSection("DiscordToken")["Token"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All
            });
            Commands = DiscordClient?.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "!", "$" },
                EnableDms = true,
                EnableMentionPrefix = true,
                Services = serviceProvider
            });
            Commands?.RegisterCommands(Assembly.GetExecutingAssembly());
            Interactivity = DiscordClient?.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(30),
                PollBehaviour = PollBehaviour.KeepEmojis,
                PaginationBehaviour = PaginationBehaviour.WrapAround,
                AckPaginationButtons = true,
                ButtonBehavior = ButtonPaginationBehavior.Disable,
                PaginationDeletion = PaginationDeletion.DeleteEmojis,
                PaginationEmojis = new PaginationEmojis
                {
                    Left = DiscordEmoji.FromName(DiscordClient, ":arrow_backward:"),
                    Right = DiscordEmoji.FromName(DiscordClient, ":arrow_forward:"),
                    SkipLeft = DiscordEmoji.FromName(DiscordClient, ":track_previous:"),
                    SkipRight = DiscordEmoji.FromName(DiscordClient, ":track_next:"),
                    Stop = DiscordEmoji.FromName(DiscordClient, ":stop_button:")
                },
                PaginationButtons = new PaginationButtons
                {
                    Left = new DiscordButtonComponent(ButtonStyle.Primary, "left", "Left", false,
                        new DiscordComponentEmoji(DiscordEmoji.FromName(DiscordClient, ":arrow_backward:"))),
                    Right = new DiscordButtonComponent(ButtonStyle.Primary, "right", "Right", false,
                        new DiscordComponentEmoji(DiscordEmoji.FromName(DiscordClient, ":arrow_forward:"))),
                    SkipLeft = new DiscordButtonComponent(ButtonStyle.Primary, "skip_left", "Skip Left", false,
                        new DiscordComponentEmoji(DiscordEmoji.FromName(DiscordClient, ":track_previous:"))),
                    SkipRight = new DiscordButtonComponent(ButtonStyle.Primary, "skip_right", "Skip Right", false,
                        new DiscordComponentEmoji(DiscordEmoji.FromName(DiscordClient, ":track_next:"))),
                    Stop = new DiscordButtonComponent(ButtonStyle.Primary, "stop", "Stop", false,
                        new DiscordComponentEmoji(DiscordEmoji.FromName(DiscordClient, ":stop_button:")))
                },
                ResponseBehavior = InteractionResponseBehavior.Respond,
                ResponseMessage = "This message is not used"
            });
            SlashCommands = DiscordClient?.UseSlashCommands(new SlashCommandsConfiguration
            {
                Services = serviceProvider
            }); 
            SlashCommands?.RegisterCommands(Assembly.GetExecutingAssembly());
            return Task.CompletedTask;
        }

        private Task ConfigureBotEvents()
        {
            return DiscordClient == null ? Task.FromException(new NullReferenceException()) : Task.CompletedTask;
        }
    }
}