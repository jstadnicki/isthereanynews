using Itan.Functions.Workers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(Itan.Functions.Startup))]
namespace Itan.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IChannelsProvider, ChannelProvider>();
            builder.Services.AddScoped<ISerializer, JsonWrapperSerializer>();

            builder.Services.AddOptions<ConnectionOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection("ConnectionStrings").Bind(settings);
               });
            
            builder.Services.AddScoped<IConnectionOptions, ConnectionOptions>();
            builder.Services.AddScoped<IFunction1Worker, Function1Worker>();
            builder.Services.AddScoped(typeof(IQueue<>), typeof(AzureQueueWrapper<>));
        }
    }
}