using System;
using System.Collections.Generic;
using System.Text;
using Quartz;

namespace Peppy.Quartz
{
    public class QuartzOptions
    {
        public IScheduler Scheduler { get; }

        public QuartzOptions(IScheduler scheduler)
        {
            Scheduler = scheduler;
        }
    }
}