using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CQRS.Domain.Commands;
using CQRS.Domain.Events;
using CQRS.Domain.Interfaces;
using CQRS.Infra.CrossCutting;
using CQRS.Infra.Data;
using CQRS.Infra.Data.MongoDB;
using CQRS.Infra.Data.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CQRS
{
    public class Startup
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"), options => options.MigrationsAssembly("CQRS")));

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddTransient<IClienteMongoDbRepository, ClienteMongoDbRepository>();
            services.AddTransient<EventPublisher>();
            services.AddSingleton<ClienteMessageListener>();
            services.AddScoped<ICommandHandler<Command>, ClienteCommandHandler>();

            services.AddControllers();
           // new AutofacServiceProvider(ConfigureAutoFac(services));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ClienteMessageListener messageListener)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            new Thread(() =>
            {
                messageListener.Start(_env.ContentRootPath);
            }).Start();

        }

        private static IContainer ConfigureAutoFac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterModule(new ApplicationModule());

            return builder.Build();
        }
    }
}
