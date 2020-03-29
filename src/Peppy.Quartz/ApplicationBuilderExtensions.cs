using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Quartz;

namespace Peppy.Quartz
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseQuartzAutostartJob<TJob>(this IApplicationBuilder applicationBuilder)
            where TJob : IJob
        {
            var applicationLifetime = applicationBuilder.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var quartzJobManager = applicationBuilder.ApplicationServices.GetRequiredService<IQuartzJobManager>();
            applicationLifetime.ApplicationStarted.Register(async () => await quartzJobManager.RunJobAsync<TJob>());
            applicationLifetime.ApplicationStopping.Register(async () => await quartzJobManager.DeleteJobAsync<TJob>());
            return applicationBuilder;
        }
    }
}