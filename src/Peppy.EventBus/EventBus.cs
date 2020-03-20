using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Peppy.EventBus
{
    public class EventBus<TEvent> : IEventBus<TEvent> where TEvent : IEvent
    {
        private readonly EventQueue eventQueue = new EventQueue();
        private readonly IEnumerable<IEventHandler<TEvent>> eventHandlers;

        public EventBus(IEnumerable<IEventHandler<TEvent>> eventHandlers)
        {
            this.eventHandlers = eventHandlers;
        }

        /// <summary>
        /// 发布事件到队列时触发处理事件
        /// </summary>
        /// <param name="sendere"></param>
        /// <param name="e"></param>
        private void EventQueue_EventPushed(object sendere, EventProcessedEventArgs e)
        {
            (from eh in this.eventHandlers
             where
                eh.CanHandle((TEvent)e.Event)
             select eh).ToList().ForEach(async eh => await eh.HandleAsync((TEvent)e.Event));
        }

        public Task publicAsync(TEvent @event, CancellationToken cancellationToken = default)
        => Task.Factory.StartNew(() => eventQueue.Push(@event));

        /// <summary>
        /// 事件订阅(订阅队列上的事件)
        /// </summary>
        public void Subscribe()
        {
            eventQueue.EventPushed += EventQueue_EventPushed;
        }
    }
}