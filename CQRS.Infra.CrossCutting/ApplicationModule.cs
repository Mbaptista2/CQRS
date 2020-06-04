using Autofac;
using CQRS.Domain.Interfaces;
using CQRS.Infra.Data;
using CQRS.Infra.Data.Base;
using System;

namespace CQRS.Infra.CrossCutting
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           
            //builder
            //    .RegisterAssemblyTypes(typeof(Repository<>).Assembly)
            //    .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<ClienteRepository>()
            .As<IClienteRepository>()
         .InstancePerLifetimeScope();

        }
    }
}
