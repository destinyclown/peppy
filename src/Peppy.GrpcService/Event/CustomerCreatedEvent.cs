using Peppy.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peppy.GrpcService.Event
{
    public class CustomerCreatedEvent : IEvent
    {
        public CustomerCreatedEvent(string customerName)
        {
            this.Id = Guid.NewGuid();
            this.Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.CustomerName = customerName;
        }

        public Guid Id { get; }

        public long Timestamp { get; }

        public string CustomerName { get; }
    }
}