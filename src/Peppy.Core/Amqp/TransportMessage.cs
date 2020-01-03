﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Core.Amqp
{
    /// <summary>
    /// Message content field
    /// </summary>
    public class TransportMessage
    {
        public TransportMessage(IDictionary<string, string> headers, byte[] body)
        {
            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
            Body = body;
        }

        /// <summary>
        /// Gets the headers of this message
        /// </summary>
        public IDictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the body object of this message
        /// </summary>
        public byte[] Body { get; }

        public string GetId()
        {
            return Headers.TryGetValue("msg-id", out var value) ? value : null;
        }

        public string GetName()
        {
            return Headers.TryGetValue("msg-name", out var value) ? value : null;
        }

        public string GetGroup()
        {
            return Headers.TryGetValue("msg-group", out var value) ? value : null;
        }
    }
}