using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Peppy.Quartz
{
    /// <summary>
    /// Quartz manage interface
    /// </summary>
    public interface IQuartzJobManager
    {
        Task RunJobAsync<TJob>(JobDataMap keyValues = null, string schedulerName = "DefaultQuartzScheduler")
            where TJob : IJob;

        Task ShopJobAsync<TJob>(string schedulerName = "DefaultQuartzScheduler")
            where TJob : IJob;

        Task DeleteJobAsync<TJob>(string schedulerName = "DefaultQuartzScheduler")
            where TJob : IJob;
    }
}
