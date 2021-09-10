using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpcHub.Da.Client.Configuration;
using OpcHub.Da.Client.Metadata;
using OpcHub.Da.Client.Services;

namespace OpcHub.Da.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpcDa(
            this IServiceCollection services,
            IConfiguration configuration,
            params Assembly[] assemblies)
        {
            services.AddOptions()
                .Configure<BlockOption>(configuration.GetSection("opcda"));

            BlockOption blockOption = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<BlockOption>>().Value;

            services.AddSingleton(BlockMetadataCollectionBuilder.Build(assemblies, blockOption))
                    .AddTransient<IReadService, ReadService>()
                    .AddTransient<IWriteService, WriteService>()
                    .AddTransient<IOpcDaContext, OpcDaContext>();

            return services;
        }
    }
}
