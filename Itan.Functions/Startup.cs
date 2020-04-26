using Itan.Functions.Workers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Itan.Functions.Startup))]
namespace Itan.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped(typeof(ILoger<>), typeof(Loger<>));
            builder.Services.AddScoped<IChannelsProvider, ChannelProvider>();
            builder.Services.AddScoped<ISerializer, JsonWrapperSerializer>();
            builder.Services.AddScoped<IStreamBlobReader, StreamReaderWrapper>();
            builder.Services.AddScoped<IFeedReader, FeedReaderWrapper>();
            builder.Services.AddScoped<INewsWriter, NewsWriter>();

            builder.Services.AddScoped<IChannelsDownloadsReader, ChannelsDownloadsReader>();
            builder.Services.AddScoped<IChannelsDownloadsWriter, ChannelsDownloadsWriter>();
            builder.Services.AddScoped<IBlobPathGenerator, BlobPathGenerator>();
            builder.Services.AddScoped<IHttpDownloader, HttpDownloader>();
            builder.Services.AddScoped<IBlobContainer, BlobContainer>();

            builder.Services.AddOptions<ConnectionOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection("ConnectionStrings").Bind(settings);
               });
            
            builder.Services.AddScoped(typeof(IQueue<>), typeof(AzureQueueWrapper<>));

            builder.Services.AddScoped<IFunction1Worker, Function1Worker>();
            builder.Services.AddScoped<IFunction2Worker, Function2Worker>();
        }
    }
}