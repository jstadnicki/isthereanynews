using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Itan.Wrappers
{
    public class ItanWrappersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ISerializer).GetTypeInfo().Assembly).AsImplementedInterfaces(); //itan.wrappers
            builder.RegisterGeneric(typeof(AzureQueueWrapper<>)).As(typeof(IQueue<>));
        }
    }
}