using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Peppy.AutoIoc;
using Peppy.Dependency;
using Peppy.Quartz;
using Peppy.Swagger;
using Peppy.RabbitMQ;
using Peppy.Redis;
using Quartz;
using Quartz.Impl;
using Sample.WebApi.Handlers;
using Swashbuckle.AspNetCore.Swagger;
using Peppy.Mapper;
using Sample.WebApi.Jobs;

namespace Sample.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly Action<Peppy.Swagger.SwaggerOptions> _swaggerOptionsAction =
            options =>
            {
                options.OpenApiInfos = new List<OpenApiInfo>
                {
                    new OpenApiInfo
                    {
                        Title = "test",
                        Version = "v1",
                        Description = "test"
                    },
                    new OpenApiInfo
                    {
                        Title = "test",
                        Version = "v2",
                        Description = "test"
                    }
                };
                options.Files = new List<string> { "Sample.WebApi.xml" };
            };

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<AppDbContext>();
            //services.AddCap(x =>
            //{
            //    x.UseEntityFramework<AppDbContext>();
            //    x.UseDashboard();
            //    x.UseRabbitMQ(configure =>
            //    {
            //        configure.HostName = "134.175.159.22";
            //        configure.UserName = "bailun";
            //        configure.Password = "bailun2019";
            //    });
            //    x.FailedRetryCount = 5;
            //});
            //services.AddPeppyRedis(options =>
            //{
            //    options.HostName = "192.168.6.45";
            //    options.Port = "6379";
            //    options.Defaultdatabase = 0;
            //});
            services.AddPeppy(options =>
            {
                //options.UseSqlSugarCore(
                //    sugar =>
                //    {
                //        sugar.DbType = SqlSugar.DbType.MySql;
                //        sugar.ConnectionString = Configuration.GetConnectionString("Default");
                //    });
            });
            services.AddEntityFrameworkCore<AppDbContext>(Configuration.GetConnectionString("Default"));
            services.AddRedis(Configuration);
            services.AddQuartzJob();
            services.AddPeppyRabbitMQ(options =>
            {
                options.HostName = "134.175.159.22";
                options.UserName = "bailun";
                options.Password = "bailun2019";
            });
            services.AddAutoIoc(typeof(IScopedDependency))
                .AddAutoIoc(typeof(ISingletonDependency), LifeCycle.Singleton)
                .AddAutoIoc(typeof(ITransientDependency), LifeCycle.Transient)
                .AddMapper();
            services.AddMediatR(typeof(IEventHandler).Assembly);
            services.AddSwagger(_swaggerOptionsAction, codeEnumType: typeof(StatusCodeEnum));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticFiles()
                .UseSwagger(_swaggerOptionsAction)
                .UseQuartzAutostartJob<MyJob>();
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();
            var name = Dns.GetHostName(); // get container id
            var ip = Dns.GetHostEntry(name).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            var uri = new Uri(address);
            var port = uri.Port;
        }
    }
}