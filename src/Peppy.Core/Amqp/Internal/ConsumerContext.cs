﻿using System;
using Peppy.Core.Amqp.Messages;
using System.Text;

namespace Peppy.Core.Amqp.Internal
{
    /// <summary>
    /// A context for consumers, it used to be provider wrapper of method description and received message.
    /// </summary>
    public class ConsumerContext
    {
        /// <summary>
        /// create a new instance of  <see cref="ConsumerContext" /> .
        /// </summary>
        /// <param name="descriptor">consumer method descriptor. </param>
        /// <param name="message"> received message.</param>
        public ConsumerContext(ConsumerExecutorDescriptor descriptor, Message message)
        {
            ConsumerDescriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
            DeliverMessage = message ?? throw new ArgumentNullException(nameof(message));
        }

        /// <summary>
        /// a descriptor of consumer information need to be performed.
        /// </summary>
        public ConsumerExecutorDescriptor ConsumerDescriptor { get; }

        /// <summary>
        /// consumer received message.
        /// </summary>
        public Message DeliverMessage { get; }
    }
}