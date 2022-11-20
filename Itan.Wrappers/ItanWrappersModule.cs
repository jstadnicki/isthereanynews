using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
namespace Itan.Wrappers
{
    public class ItanWrappersModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public ItanWrappersModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ISerializer).GetTypeInfo().Assembly).AsImplementedInterfaces(); //itan.wrappers
            builder.RegisterGeneric(typeof(AzureQueueWrapper<>)).As(typeof(IQueue<>));
            
            builder.Register<IOptions<EmailSenderSettings>>(context =>
                {
                    var s = _configuration
                        .GetSection("EmailSenderSettings")
                        .Get<EmailSenderSettings>();
                    return new OptionsWrapper<EmailSenderSettings>(s);
                })
                .SingleInstance();
            
        }
    }
}