﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MURDoX.Core.Services;
using MURDoX.DiscordAccess.Commands.EventHandlers;
using Remora.Discord.API.Abstractions.Gateway.Commands;
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
            
            ServiceProvider services = new ServiceCollection()
                .AddDiscordGateway(_ => configuration.GetSection("DiscordToken")["Token"] ?? throw new InvalidOperationException("Token is null"))
                .Configure<DiscordGatewayClientOptions>(g => g.Intents |= GatewayIntents.MessageContents)
                .AddSingleton<SuggestionService>()
                .AddSingleton<WelcomeService>()
                .BuildServiceProvider();

            Console.WriteLine("Connected");
            DiscordGatewayClient gatewayClient = services.GetRequiredService<DiscordGatewayClient>();
            ILogger<Program> log = services.GetRequiredService<ILogger<Program>>();

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