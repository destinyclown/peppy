using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.EventBus
{
    public interface IEventSubscriber
    {
        /// <summary>
        /// 事件订阅
        /// </summary>
        void Subscribe();
    }
}