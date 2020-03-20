using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.EventBus
{
    public class EventQueue
    {
        public event EventHandler<EventProcessedEventArgs> EventPushed;

        public EventQueue()
        {
        }

        public void Push(IEvent @event)
        {
            OnMessagePushed(new EventProcessedEventArgs(@event));
        }

        private void OnMessagePushed(EventProcessedEventArgs e)
        {
            this.EventPushed?.Invoke(this, e);
        }
    }
}