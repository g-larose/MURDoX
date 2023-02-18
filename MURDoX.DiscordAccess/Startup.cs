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
using Remora.Discord.Commands.Responders;
using Remora.Discord.Extensions.Extensions;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;
using Remora.Discord.Gateway.Results;
using Remora.Results;

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

            var provider = new ServiceCollection()
                .AddDiscordGateway(_ =>
                    configuration.GetSection("DiscordToken")["Token"] ??
                    throw new InvalidOperationException("Token is null"))
                .AddDiscordCommands(true)
                .AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
                .AddCommandGroupsFromAssembly(Assembly.GetExecutingAssembly())
                .Configure<DiscordGatewayClientOptions>(g => g.Intents |= GatewayIntents.MessageContents)
                .Configure<DiscordGatewayClientOptions>(g => g.Intents |= GatewayIntents.Guilds)
                .AddSingleton<SuggestionService>()
                .AddSingleton<DiceRollerGameService>()
                .BuildServiceProvider();

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