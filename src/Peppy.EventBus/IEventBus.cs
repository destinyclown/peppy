using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.EventBus
{
    public interface IEventBus<TEvent> : IEventSubscriber, IEventpublicer<TEvent> where TEvent : IEvent
    {
    }
}