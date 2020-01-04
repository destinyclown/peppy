using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Core.Amqp.Internal
{
    public interface ISubscribeInvokerFactory
    {
        ISubscribeInvoker CreateInvoker();
    }
}