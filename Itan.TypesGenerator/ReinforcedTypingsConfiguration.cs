using Itan.Common;
using Itan.Core;
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

            builder.ExportAsClass<ChannelViewModel>()
                .WithAllProperties();
            
            builder.ExportAsClass<NewsViewModel>()
                .WithAllProperties();

            builder.ExportAsClass<HomePageNewsViewModel>()
                .WithAllProperties();

            builder.ExportAsClass<LandingPageNewsViewModel>()
                .WithAllProperties();
        }
    }
}