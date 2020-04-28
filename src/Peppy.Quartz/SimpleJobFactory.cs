using System;
using Quartz.Logging;
using Quartz.Spi;
using Quartz.Util;
namespace Quartz.Simpl
{
    ///  
    /// The default JobFactory used by Quartz - simply calls 
    /// 
    /// 
    /// Marko Lahma (.NET)
    public class SimpleJobFactory : IJobFactory
    {
        /// 
        /// Called by the scheduler at the time of the trigger firing, in order to
        /// produce a 
        /// 
        /// It should be extremely rare for this method to throw an exception -
        /// basically only the case where there is no way at all to instantiate
        /// and prepare the Job for execution. When the exception is thrown, the
        /// Scheduler will move all triggers associated with the Job into the
        /// 
        /// The TriggerFiredBundle from which the 
        /// 
        /// the newly instantiated Job
        ///  SchedulerException if there is a problem instantiating the Job. 
        public virtual IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJobDetail jobDetail = bundle.JobDetail;
            Type jobType = jobDetail.JobType;
            try
            {

                return ObjectUtils.InstantiateType(jobType);
            }
            catch (Exception e)
            {
                SchedulerException se = new SchedulerException($"Problem instantiating class '{jobDetail.JobType.FullName}'", e);
                throw se;
            }
        }

        /// 
        /// Allows the job factory to destroy/cleanup the job if needed. 
        /// No-op when using SimpleJobFactory.
        /// 
        public virtual void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
