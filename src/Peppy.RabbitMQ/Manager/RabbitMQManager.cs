using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Peppy.Core;
using Peppy.Core.Amqp;
using Peppy.Core.Amqp.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Peppy.RabbitMQ.Manager
{
    internal sealed class RabbitMQManager : IRabbitMQManager
    {
        private readonly ILogger<RabbitMQManager> _logger;
        private readonly IOptions<PeppyRabbitMQOptions> _rabbitMQOptions;
        private readonly ConnectionFactory _connectionFactory;
        private static IConnection _connection;
        private static string _connStr;
        private IModel _channel;

        public event EventHandler<TransportMessage> OnMessageReceived;

        public RabbitMQManager(
            ILogger<RabbitMQManager> logger,
            IOptions<PeppyRabbitMQOptions> rabbitMQOptions)
        {
            _logger = logger;
            _rabbitMQOptions = rabbitMQOptions;
            _connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitMQOptions.Value.HostName,
                Port = _rabbitMQOptions.Value.Port,
                UserName = _rabbitMQOptions.Value.UserName,
                Password = _rabbitMQOptions.Value.Password
            };
            _connStr = string.Format("{0}:{1},userName={2},password={3}",
                _rabbitMQOptions.Value.HostName,
                _rabbitMQOptions.Value.Port,
                _rabbitMQOptions.Value.UserName,
                _rabbitMQOptions.Value.Password
            );
        }

        public void Listening(string exchangeName, string queueName)
        {
            _channel = Connect(exchangeName, queueName);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnConsumerReceived;
            _channel.BasicConsume(queueName, false, consumer);
        }

        private void CreateConn(string name)
        {
            try
            {
                _logger.LogDebug($"RabbitMQ config: {_connStr}");
                _connection = _connectionFactory.CreateConnection(name);
                _logger.LogInformation("RabbitMQ manager started!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"RabbitMQ connection error: {ex.Message}");
            }
        }

        private IModel GetChannel(string name)
        {
            if (_connection == null || !_connection.IsOpen)
            {
                CreateConn(name);
            }
            var channel = _connection.CreateModel();
            return channel;
        }

        public void CreateConnect(string exchangeName, string queueName)
        {
            Connect(exchangeName, queueName);
        }

        private IModel Connect(string exchangeName, string queueName)
        {
            _channel = GetChannel(queueName);
            if (!string.IsNullOrEmpty(exchangeName))
            {
                //声明交换机
                _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);
                //声明一个队列
                _channel.QueueDeclare(queueName, true, false, false);
                //绑定队列，交换机，路由键
                _channel.QueueBind(queueName, exchangeName, queueName);
            }
            else
            {
                //声明一个队列
                _channel.QueueDeclare(queueName, true, false, false);
            }
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            return _channel;
        }

        private void OnConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var headers = new Dictionary<string, string>();
            //foreach (var header in e.BasicProperties.Headers)
            //{
            //    headers.Add(header.Key, header.Value == null ? null : Encoding.UTF8.GetString((byte[])header.Value));
            //}

            var message = new TransportMessage(headers, e.Body);

            OnMessageReceived?.Invoke(e.DeliveryTag, message);
        }

        public void Commit(object sender)
        {
            _channel.BasicAck((ulong)sender, false);
        }

        public void Reject(object sender)
        {
            _channel.BasicReject((ulong)sender, true);
        }

        public void Receive(string exchangeName, string queueName, Action<object> received)
        {
            try
            {
                {
                    _channel = Connect(exchangeName, queueName);
                    //事件基本消费者
                    var consumer = new EventingBasicConsumer(_channel);
                    //接收到消息事件
                    consumer.Received += (ch, ea) =>
                    {
                        var type = received.Method.GetParameters().FirstOrDefault().ParameterType;
                        string message = Encoding.UTF8.GetString(ea.Body);
                        var msg = message.ToObject<Message>();
                        DateTime time = DateTime.Now;
                        received(msg.Value);
                        var timeEnd = DateTime.Now - time;
                        if (_channel.IsClosed)
                        {
                            return;
                        }
                        _logger.LogInformation($"RabbitMQ receive success, time cost  {timeEnd.TotalSeconds.ToString("0.00")}s {queueName} remaining messages： {_channel.MessageCount(queueName)}");
                        //确认该消息已被消费
                        _channel.BasicAck(ea.DeliveryTag, false);
                    };
                    //启动消费者 设置为手动应答消息
                    _channel.BasicConsume(queueName, false, consumer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"RabbitMQ Error: {ex.Message}");
            }
        }

        public async Task ReceiveAsync(string exchangeName, string queueName, Action<object> received)
        {
            await Task.Run(() =>
            {
                Receive(exchangeName, queueName, received);
            });
        }

        public bool SendMessages<T>(string exchangeName, string queueName, IList<T> msgs)
        {
            if (msgs == null && !msgs.Any())
            {
                return false;
            }
            try
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    CreateConn(queueName);
                }
                using (var channel = _connection.CreateModel())
                {
                    if (!string.IsNullOrEmpty(exchangeName))
                    {
                        //声明交换机
                        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);
                        //声明一个队列
                        channel.QueueDeclare(queueName, true, false, false);
                        //绑定队列，交换机，路由键
                        channel.QueueBind(queueName, exchangeName, queueName);
                    }
                    else
                    {
                        //声明一个队列
                        channel.QueueDeclare(queueName, true, false, false, null);
                    }
                    var basicProperties = channel.CreateBasicProperties();
                    //1：非持久化 2：可持久化
                    basicProperties.DeliveryMode = 2;
                    var address = new PublicationAddress(ExchangeType.Direct, exchangeName, queueName);
                    var headers = new Dictionary<string, string>
                    {
                        { Core.Amqp.Messages.Headers.Type, typeof(T).FullName },
                        { Core.Amqp.Messages.Headers.SentTime, DateTimeOffset.Now.ToString() }
                    };
                    foreach (var msg in msgs)
                    {
                        var message = new Message(headers, msg);
                        var payload = Encoding.UTF8.GetBytes(message.ToJson());
                        channel.BasicPublish(address, basicProperties, payload);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"RabbitMQ Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendMessagesAsync<T>(string exchangeName, string queueName, IList<T> msgs)
        {
            return await Task.Run(() =>
            {
                return SendMessages(exchangeName, queueName, msgs);
            });
        }

        public bool SendMsg<T>(string exchangeName, string queueName, T msg)
        {
            if (msg == null)
            {
                return false;
            }
            try
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    CreateConn(queueName);
                }
                using (var channel = _connection.CreateModel())
                {
                    if (!string.IsNullOrEmpty(exchangeName))
                    {
                        //声明交换机
                        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);
                        //声明一个队列
                        channel.QueueDeclare(queueName, true, false, false);
                        //绑定队列，交换机，路由键
                        channel.QueueBind(queueName, exchangeName, queueName);
                    }
                    else
                    {
                        //声明一个队列
                        channel.QueueDeclare(queueName, true, false, false, null);
                    }
                    var headers = new Dictionary<string, string>
                    {
                        { Core.Amqp.Messages.Headers.Type, typeof(T).FullName },
                        { Core.Amqp.Messages.Headers.SentTime, DateTimeOffset.Now.ToString() }
                    };
                    var basicProperties = channel.CreateBasicProperties();
                    //1：非持久化 2：可持久化
                    basicProperties.DeliveryMode = 2;
                    var message = new Message(headers, msg);
                    var transportMessage = new TransportMessage(message.Headers, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message.Value)));
                    var payload = transportMessage.Body;
                    var address = new PublicationAddress(ExchangeType.Direct, exchangeName, queueName);
                    channel.BasicPublish(address, basicProperties, payload);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"RabbitMQ Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendMsgAsync<T>(string exchangeName, string queueName, T msg)
        {
            return await Task.Run(() =>
            {
                return SendMsg(exchangeName, queueName, msg);
            });
        }
    }
}