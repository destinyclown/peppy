using System;

namespace Peppy.Core.Amqp
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public abstract class AmqpAttribute : Attribute
    {
        protected AmqpAttribute(string exchangeName, string queueName)
        {
            ExchangeName = exchangeName;
            QueueName = queueName;
        }

        /// <summary>
        /// Exchange Name.
        /// </summary>
        public string ExchangeName { get; }

        /// <summary>
        /// Queue Name
        /// </summary>
        public string QueueName { get; set; }
    }
}