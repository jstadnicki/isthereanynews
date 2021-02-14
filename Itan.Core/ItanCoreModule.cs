using System.Reflection;
using Autofac;
using Itan.Core.CreateNewUser;
using Itan.Core.DeleteAccount;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Module = Autofac.Module;

namespace Itan.Core
{
    public class ItanCoreModule : Module
    {
        private readonly IConfiguration configuration;

        public ItanCoreModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ICreateUserRepository).GetTypeInfo().Assembly)
                .AsImplementedInterfaces(); // via assembly scan

            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<IOptions<GraphApiSettings>>(context =>
                {
                    var s = this.configuration
                        .GetSection("GraphApi")
                        .Get<GraphApiSettings>();
                    return new OptionsWrapper<GraphApiSettings>(s);
                })
                .SingleInstance();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}