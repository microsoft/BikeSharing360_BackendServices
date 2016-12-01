using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BikeSharing.Services.Events.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.Swagger.Model;
using BikeSharing.Services.Events.Queries;

namespace BikeSharing.Services.Events
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var constr = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<CityEventsDbContext>
                (options => options.UseSqlServer(constr));

            services.AddTransient<IEventsQueries, EventsQueries>();
            // Add framework services.
            services.AddMvc();
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "BikeSharing 360 Events API",
                    Description = "Events API for MS Connect 2016 Demos",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseMvc();

            // Seed the EF database
            app.ApplicationServices.GetRequiredService<CityEventsDbContext>().Seed();
        }
    }
}
