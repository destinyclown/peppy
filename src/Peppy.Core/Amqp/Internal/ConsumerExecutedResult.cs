﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Core.Amqp.Internal
{
    public class ConsumerExecutedResult
    {
        public ConsumerExecutedResult(object result, string msgId, string callbackName)
        {
            Result = result;
            MessageId = msgId;
            CallbackName = callbackName;
        }

        public object Result { get; set; }

        public string MessageId { get; set; }

        public string CallbackName { get; set; }
    }
}