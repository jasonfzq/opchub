using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpcHub.Da.Client.Test.Blocks;

namespace OpcHub.Da.Client.Test
{
    //https://dfederm.com/building-a-console-app-with-.net-generic-host/
    internal class Program
    {
        private static async Task Main(string[] args)
        {
           
            await Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, builder) => { builder.AddJsonFile("opcda.json"); })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddHostedService<ConsoleHostedService>();
                    services.AddOpcDa(hostContext.Configuration, Assembly.GetExecutingAssembly());
                })
                .RunConsoleAsync();
        }
    }

    internal sealed class ConsoleHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public ConsoleHostedService(
            ILogger<ConsoleHostedService> logger,
            IHostApplicationLifetime appLifetime,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            _appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation("Testing start......");

                        var context = _serviceProvider.GetRequiredService<IOpcDaContext>();

                        //var tank = await context.Read<TankBlock>("TANK_STS_A");
                        var block = await context.Read<WeighingReadBlock>("WEIGHING");
                        

                        //var data = await context.Read<ShipUnloadingOperationReadBlock>();
                        

                        //await context.Write(block);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled exception!");
                    }
                    finally
                    {
                        // Stop the application once the work is done
                        _appLifetime.StopApplication();
                    }
                });
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}