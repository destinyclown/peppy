using Peppy.Core.Amqp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Peppy
{
    public class RabbitMQReceiveAttribute : AmqpAttribute
    {
        public RabbitMQReceiveAttribute(string exchangeName, string queueName)
            : base(exchangeName, queueName)
        {
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        public class FromPeppyAttribute : Attribute
        {
        }

        public class PeyypHeader : ReadOnlyDictionary<string, string>
        {
            public PeyypHeader(IDictionary<string, string> dictionary) : base(dictionary)
            {
            }
        }
    }
}