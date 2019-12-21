using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Peppy.EventBus
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> HandleAsync(TEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// 可否处理
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        bool CanHandle(TEvent @event);
    }
}