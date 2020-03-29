using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Quartz
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class QuartzJobAttribute : Attribute
    {
        public string Cron { get; set; }

        public string Name { get; set; }

        public string Group { get; set; }

        public string Description { get; set; }

        public Action<SimpleScheduleBuilder> Action { get; set; }

        public QuartzJobAttribute(
            string name,
            string group,
            string description,
            string cron = ""
        )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrEmpty(group))
            {
                throw new ArgumentNullException(nameof(group));
            }
            Name = name;
            Group = group;
            Cron = cron;
            Description = description;
        }
    }
}