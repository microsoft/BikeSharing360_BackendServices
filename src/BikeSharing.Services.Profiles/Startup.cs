using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BikeSharing.Services.Profiles.Data;
using Microsoft.EntityFrameworkCore;
using BikeSharing.Services.Profiles.Queries;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BikeSharing.Services.Profiles.Commands;
using System.Reflection;
using Swashbuckle.Swagger.Model;
using BikeSharing.Services.Core.Commands;
using MyBikes.Services.Profiles.Formatters;

namespace BikeSharing.Services.Profiles
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var constr = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<ProfilesDbContext>
                (options => options.UseSqlServer(constr));
            services.AddTransient<IProfileQueries, ProfileQueries>();
            services.AddTransient<IUserQueries, UserQueries>();
            services.AddTransient<ITenantQueries, TenantQueries>();
            services.AddTransient<ISubscriptionsQueries, SubscriptionsQueries>();
            services.AddSingleton<IConfiguration>(Configuration);
            // Add framework services.
            services.AddCors();
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .AddMvcOptions(options =>
            {
                options.InputFormatters.Insert(0, new TextPlainInputFormatter());
            });

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "BikeSharing 360 Profiles API",
                    Description = "Profiles API for MS Connect 2016 Demos",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
            });
            // Plug Autofac
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<ProfilesCommandBus>().As<ICommandBus>();
            // Register CommandHandlers
            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                 .AsClosedTypesOf(typeof(ICommandHandler<>))
                 .AsImplementedInterfaces()
                 .InstancePerLifetimeScope();

            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors(b => b.AllowAnyOrigin());
            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseMvc();
        }
    }
}
