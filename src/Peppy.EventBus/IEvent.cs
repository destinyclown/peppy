using System;

namespace Peppy.EventBus
{
    public interface IEvent
    {
        Guid Id { get; }

        /// <summary>
        /// 时间戳
        /// </summary>
        long Timestamp { get; }
    }
}