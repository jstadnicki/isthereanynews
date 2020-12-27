using System.Reflection;
using Autofac;
using Itan.Core.CreateNewUser;
using Itan.Wrappers;
using MediatR;
using Module = Autofac.Module;

namespace Itan.Core
{
    public class ItanCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
             builder.RegisterAssemblyTypes(typeof(ICreateUserRepository).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan

             builder
                 .RegisterType<Mediator>()
                 .As<IMediator>()
                 .InstancePerLifetimeScope();

             builder.Register<ServiceFactory>(context =>
             {
                 var c = context.Resolve<IComponentContext>();
                 return t => c.Resolve(t);
             });
        }
    }
}