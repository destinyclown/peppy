using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.EventBus
{
    public interface IEventBus<TEvent> : IEventSubscriber, IEventPublisher<TEvent> where TEvent : IEvent
    {
    }
}