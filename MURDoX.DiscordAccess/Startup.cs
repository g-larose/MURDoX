#region

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MURDoX.Core.Data;
using MURDoX.Core.Services;
using MURDoX.DiscordAccess.Commands.TextCommands;
using Remora.Commands.Extensions;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Extensions.Extensions;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;
using Remora.Discord.Gateway.Results;
using Remora.Results;

#endregion

namespace MURDoX.DiscordAccess
{
    public static class Startup
    {
        public static async Task RunAsync()
        {
            CancellationTokenSource cancellationSource = new();
            Console.CancelKeyPress += (_, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cancellationSource.Cancel();
            };

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            ServiceProvider provider = new ServiceCollection()
                .AddDiscordGateway(_ =>
                    configuration.GetSection("DiscordToken")["Token"] ??
                    throw new InvalidOperationException("Token is null"))
                .AddDbContext<ApplicationDbContext>()
                .AddSingleton<ApplicationDbContextFactory>() 
                //.AddCommandGroupsFromAssembly(Assembly.GetExecutingAssembly())
                .AddCommandTree().WithCommandGroup<PingPongTest>().Finish()
                .AddCommandTree().WithCommandGroup<UtilityCommandGroup>().Finish()
                .AddCommandTree().WithCommandGroup<EconomyGameCommandGroup>().Finish()
                .AddCommandTree().WithCommandGroup<DiceRollerCommandGroup>().Finish()
                .Configure<DiscordGatewayClientOptions>(options =>
                {
                    options.Intents |= GatewayIntents.MessageContents;
                })
                .AddSingleton<SuggestionService>()
                .AddSingleton<DiceRollerGameService>()
                .AddSingleton<EconomyGameService>()
                .AddDiscordCommands()
                .BuildServiceProvider();

            // print the name of all assemblies that are loaded
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Console.WriteLine(assembly.FullName);
            }
            Console.WriteLine("Connected");

            DiscordGatewayClient gatewayClient = provider.GetRequiredService<DiscordGatewayClient>();
            ILogger<Program> log = provider.GetRequiredService<ILogger<Program>>();

            Result runResult = await gatewayClient.RunAsync(cancellationSource.Token);

            if (!runResult.IsSuccess)
            {
                switch (runResult.Error)
                {
                    case ExceptionError exe:
                    {
                        log.LogError
                        (
                            exe.Exception,
                            "Exception during gateway connection: {ExceptionMessage}",
                            exe.Message
                        );

                        break;
                    }
                    case GatewayWebSocketError:
                    case GatewayDiscordError:
                    {
                        log.LogError("Gateway error: {Message}", runResult.Error.Message);
                        break;
                    }
                    default:
                    {
                        log.LogError("Unknown error: {Message}", runResult.Error.Message);
                        break;
                    }
                }
            }

            Console.WriteLine("Bye bye");
        }
    }
}