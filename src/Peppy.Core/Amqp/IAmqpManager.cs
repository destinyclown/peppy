using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peppy.Core.Amqp
{
    public interface IAmqpManager
    {
        void CreateConnect(string exchangeName, string queueName);

        void Commit(object sender);

        void Reject(object sender);

        void Listening(string exchangeName, string queueName);

        void Receive(string exchangeName, string queueName, Action<object> received);

        Task ReceiveAsync(string exchangeName, string queueName, Action<object> received);

        bool SendMsg<T>(string exchangeName, string queueName, T msg);

        Task<bool> SendMsgAsync<T>(string exchangeName, string queueName, T msg);

        bool SendMessages<T>(string exchangeName, string queueName, IList<T> msgs);

        Task<bool> SendMessagesAsync<T>(string exchangeName, string queueName, IList<T> msgs);

        event EventHandler<TransportMessage> OnMessageReceived;
    }
}