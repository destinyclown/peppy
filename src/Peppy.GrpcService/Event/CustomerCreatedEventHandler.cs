using Peppy.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Peppy.GrpcService.Event
{
    public class CustomerCreatedEventHandler : IEventHandler<CustomerCreatedEvent>
    {
        public bool CanHandle(CustomerCreatedEvent @event)
            => @event.GetType().Equals(typeof(CustomerCreatedEvent));

        public Task<bool> HandleAsync(CustomerCreatedEvent @event, CancellationToken cancellationToken = default)

        => CanHandle(@event) ? HandleAsync(@event, cancellationToken) : Task.FromResult(false);

        public Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }
}