using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Quartz
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseQuartzJob(this IApplicationBuilder applicationBuilder)
        {
            var applicationLifetime = applicationBuilder.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var quartz = applicationBuilder.ApplicationServices.GetRequiredService<QuartzStartup>();
            applicationLifetime.ApplicationStarted.Register(quartz.Start);
            applicationLifetime.ApplicationStopped.Register(quartz.Stop);
            return applicationBuilder;
        }
    }
}