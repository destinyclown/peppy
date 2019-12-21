using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Peppy.EventBus;
using Peppy.GrpcService.Event;

namespace Peppy.GrpcService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly IEventBus<CustomerCreatedEvent> _eventBus;
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger, IEventBus<CustomerCreatedEvent> eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            await _eventBus.PublishAsync(new CustomerCreatedEvent("test"));
            return await Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}