using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Peppy.EventBus
{
    public interface IEventpublicer<TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task publicAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}