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
    }
}