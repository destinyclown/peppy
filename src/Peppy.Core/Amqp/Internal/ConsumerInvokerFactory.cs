using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Core.Amqp.Internal
{
    public class ConsumerInvokerFactory : ISubscribeInvokerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceProvider _serviceProvider;

        public ConsumerInvokerFactory(
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            _loggerFactory = loggerFactory;
            _serviceProvider = serviceProvider;
        }

        public ISubscribeInvoker CreateInvoker()
        {
            return new SubscribeInvoker(_loggerFactory, _serviceProvider);
        }
    }
}