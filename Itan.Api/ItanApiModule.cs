using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Itan.Api.Controllers;
using Itan.Api.Middleware;
using Itan.Core.CreateNewUser;
using MediatR;
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