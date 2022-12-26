using Itan.Common;
using Itan.Core;
using Itan.Core.GetAllChannels;
using Itan.Core.GetAllReaders;
using Itan.Core.GetAllSubscribedChannels;
using Itan.Core.GetFollowerActivity;
using Itan.Core.GetFollowers;
using Itan.Core.GetHomePageNews;
using Itan.Core.GetNewsByChannel;
using Itan.Core.GetReader;
using Itan.Core.MarkNewsRead;
using Reinforced.Typings.Fluent;

namespace Itan.TypesGenerator
{
    public class ReinforcedTypingsConfiguration
    {
        public static void Configure(ConfigurationBuilder builder)
        {
            builder.Global(c=>c.DontWriteWarningComment()
                .CamelCaseForMethods()
                .CamelCaseForProperties()
                .UseModules()); 

            builder.ExportAsEnum<SquashUpdate>();
            builder.ExportAsEnum<UpdatedNews>();
            builder.ExportAsEnum<IMarkNewsReadRepository.NewsReadType>();

            builder.ExportAsClass<ChannelViewModel>()
                .WithAllProperties();
            
            builder.ExportAsClass<SubscribedChannelViewModel>()
                .WithAllProperties();

            builder.ExportAsClass<NewsHeaderTagViewModel>().WithAllProperties();
            
            builder.ExportAsClass<NewsViewModel>()
                .WithAllProperties();

            builder.ExportAsClass<HomePageNewsViewModel>()
                .WithAllProperties();

            builder.ExportAsClass<LandingPageNewsViewModel>()
                .WithAllProperties();

            builder.ExportAsClass<ReaderViewModel>()
                .WithAllProperties();

            builder.ExportAsClass<ReaderDetailsViewModel>()
                .WithAllProperties();

            builder.ExportAsClass<ReaderSubscribedChannel>()
                .WithAllProperties();

            builder.ExportAsClass<SubscribedReaderViewModel>()
                .WithAllProperties();

            builder.ExportAsClass<FollowerActivityViewModel>()
                .WithAllProperties();
        }
    }
}