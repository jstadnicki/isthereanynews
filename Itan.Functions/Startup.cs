using System;
using Azure.Identity;

using Itan.Common;
using Itan.Functions.Workers;
using Itan.Wrappers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(Itan.Functions.Startup))]

namespace Itan.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddAzureKeyVault(new Uri("https://itan-key-vault.vault.azure.net"),new DefaultAzureCredential());

            var executionContextOptions = builder.Services.BuildServiceProvider()
                .GetService<IOptions<ExecutionContextOptions>>().Value;

            var currentDirectory = executionContextOptions.AppDirectory;
            
            configurationBuilder
                .SetBasePath(currentDirectory)
                .AddJsonFile("local.settings.json", true);
            var configuration = configurationBuilder.Build();
            
            builder
                .Services
                .AddOptions<ConnectionOptions>()
                .Bind(configuration.GetSection("ConnectionStrings"));

            builder.Services.AddScoped(typeof(ILoger<>), typeof(Loger<>));
            builder.Services.AddScoped<IChannelsProvider, ChannelProvider>();
            builder.Services.AddScoped<ISerializer, JsonWrapperSerializer>();
            builder.Services.AddScoped<IStreamBlobReader, StreamReaderWrapper>();
            builder.Services.AddScoped<IFeedReader, FeedReaderWrapper>();
            builder.Services.AddScoped<INewsWriter, NewsWriter>();
            builder.Services.AddScoped<IChannelUpdater, ChannelUpdater>();

            builder.Services.AddScoped<IChannelsDownloadsReader, ChannelsDownloadsReader>();
            builder.Services.AddScoped<IChannelsDownloadsWriter, ChannelsDownloadsWriter>();
            builder.Services.AddScoped<IBlobPathGenerator, BlobPathGenerator>();
            builder.Services.AddScoped<IHttpDownloader, HttpDownloader>();
            builder.Services.AddScoped<IBlobContainer, BlobContainer>();
            builder.Services.AddScoped<IHashSum, Sha256Wrapper>();

            builder.Services.AddScoped(typeof(IQueue<>), typeof(AzureQueueWrapper<>));

            builder.Services.AddScoped<IFunction1Worker, Function1Worker>();
            builder.Services.AddScoped<IFunction2Worker, Function2Worker>();
            builder.Services.AddScoped<IFunction3Worker, Function3Worker>();
            builder.Services.AddScoped<IFunction4Worker, Function4Worker>();
        }
    }
}