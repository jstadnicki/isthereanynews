using Autofac;
using Itan.Api.Middleware;
using Module = Autofac.Module;

namespace Itan.Api
{
    public class ItanApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ItanExceptionMiddleware>();
        }
    }
}