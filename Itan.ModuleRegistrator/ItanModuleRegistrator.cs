using Autofac;
using Itan.Core;
using Itan.Wrappers;
using Microsoft.Extensions.Configuration;
using Module = Autofac.Module;

namespace Itan.ModuleRegistrator
{
    public class ItanModuleRegistrator : Module
    {
        private readonly IConfiguration _configuration;

        public ItanModuleRegistrator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ItanCoreModule(_configuration));
            builder.RegisterModule(new ItanWrappersModule(_configuration));
        }
    }
}