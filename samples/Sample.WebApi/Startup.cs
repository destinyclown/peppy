using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peppy;
using Peppy.Autofac;
using Peppy.Dependency;
using Peppy.Extensions;

namespace Sample.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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
                options.UseEntityFrameworkCore<AppDbContext>();
            });
            services.AddRedis(Configuration);

            services.AddPeppyRabbitMQ(options =>
            {
                options.HostName = "134.175.159.22";
                options.UserName = "bailun";
                options.Password = "bailun2019";
            });
            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new PeppyModule(typeof(IDependency), new Assembly[] { Assembly.GetExecutingAssembly() }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
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