using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Peppy.GrpcService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly ICappublicer _capBus;

        public GreeterService(ILogger<GreeterService> logger, ICappublicer cappublicer)
        {
            _logger = logger;
            _capBus = cappublicer;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            _capBus.("xxx.services.show.time", DateTime.Now);
            return await Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}