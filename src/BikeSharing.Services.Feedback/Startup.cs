using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BikeSharing.Services.Feedback.Api.Data;
using Microsoft.EntityFrameworkCore;
using BikeSharing.Services.Feedback.Api.Queries;
using Swashbuckle.Swagger.Model;
using BikeSharing.Services.Core.Commands;
using Autofac;
using BikeSharing.Services.Feedback.Api.StdHost.Commands;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;

namespace BikeSharing.Services.Feedback.Api
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
            services.AddDbContext<FeedbackDbContext>
                (options => options.UseSqlServer(constr));
            services.AddMvc().AddJsonOptions(o =>
            {
                o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "BikeSharing 360 Feedback API",
                    Description = "Events API for MS Connect 2016 Demos",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
            });

            // Plug Autofac
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<FeedbackCommandBus>().As<ICommandBus>();
            builder.RegisterType<IssuesQueries>().As<IIssuesQueries>();
            builder.RegisterInstance<IConfiguration>(Configuration);
            builder.RegisterType<RidesQueries>().As<IRidesQueries>();
            builder.RegisterType<UserQueries>().As<IUserQueries>();

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
            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseMvc();
        }
    }
}
