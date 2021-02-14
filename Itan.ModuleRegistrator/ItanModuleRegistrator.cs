using Autofac;
using Itan.Core;
using Itan.Wrappers;
using Microsoft.Extensions.Configuration;
using Module = Autofac.Module;

namespace Itan.ModuleRegistrator
{
    public class ItanModuleRegistrator : Module
    {
        private readonly IConfiguration configuration;

        public ItanModuleRegistrator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ItanCoreModule(this.configuration));
            builder.RegisterModule(new ItanWrappersModule(this.configuration));
        }
    }
}