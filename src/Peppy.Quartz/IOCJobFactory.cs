using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Quartz
{
    using IOCContainer = IServiceProvider;

    /// <summary>
    /// IOCJobFactory ：实现在Timer触发的时候注入生成对应的Job组件
    /// </summary>
    public class IOCJobFactory : IJobFactory
    {
        protected readonly IOCContainer Container;

        public IOCJobFactory(IOCContainer container)
        {
            Container = container;
        }

        //Called by the scheduler at the time of the trigger firing, in order to produce
        //a Quartz.IJob instance on which to call Execute.
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return Container.GetService(bundle.JobDetail.JobType) as IJob;
        }

        // Allows the job factory to destroy/cleanup the job if needed.
        public void ReturnJob(IJob job)
        {
        }
    }
}