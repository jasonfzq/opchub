using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpcHub.Ae.Client.Configuration;

namespace OpcHub.Ae.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpcAe(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions()
                .Configure<AeBlockOption>(configuration.GetSection("opcae"));

            services.AddSingleton(provider =>
            {
                AeBlockOption aeBlockOption = provider.GetService<IOptions<AeBlockOption>>().Value;
                return new OpcEventTable(aeBlockOption);
            });

            services.BuildServiceProvider().GetService<OpcEventTable>();

            return services;
        }
    }
}
