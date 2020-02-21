using Microsoft.Extensions.DependencyInjection;
using Peppy.Core;
using Peppy.Core.Amqp;
using Peppy.Core.Amqp.Internal;
using Peppy.Core.Amqp.Messages;
using Peppy.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using static Peppy.Core.Amqp.AmqpAttribute;
using static Peppy.RabbitMQReceiveAttribute;

namespace Peppy.RabbitMQ
{
    public class ClientRegister
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMQManager _rabbitMqManager;
        private ISubscribeInvoker Invoker { get; }

        public ClientRegister(
            IServiceProvider serviceProvider,
            IRabbitMQManager rabbitMQManager)
        {
            _serviceProvider = serviceProvider;
            _rabbitMqManager = rabbitMQManager;
            Invoker = _serviceProvider.GetService<ISubscribeInvokerFactory>().CreateInvoker();
            Start();
        }

        public void Start()
        {
            var groupingMatches = FindFromControllerTypes();
            foreach (var matchGroup in groupingMatches)
            {
                RegisterMessageProcessor(matchGroup);
                _rabbitMqManager.Listening(matchGroup.Attribute.ExchangeName, matchGroup.Attribute.QueueName);
            }
        }

        private IEnumerable<ConsumerExecutorDescriptor> FindFromControllerTypes()
        {
            var executorDescriptorList = new List<ConsumerExecutorDescriptor>();
            var types = Assembly.GetEntryAssembly()?.ExportedTypes;
            if (types == null) return executorDescriptorList;
            foreach (var type in types)
            {
                var typeInfo = type.GetTypeInfo();
                if (Helper.IsController(typeInfo))
                {
                    executorDescriptorList.AddRange(GetTopicAttributesDescription(typeInfo));
                }
            }

            return executorDescriptorList;
        }

        private IEnumerable<ConsumerExecutorDescriptor> GetTopicAttributesDescription(TypeInfo typeInfo, TypeInfo serviceTypeInfo = null)
        {
            foreach (var method in typeInfo.DeclaredMethods)
            {
                var topicAttr = method.GetCustomAttributes<AmqpAttribute>(true);
                var topicAttributes = topicAttr as IList<AmqpAttribute> ?? topicAttr.ToList();

                if (!topicAttributes.Any())
                {
                    continue;
                }

                foreach (var attr in topicAttributes)
                {
                    //SetSubscribeAttribute(attr);

                    var parameters = method.GetParameters()
                        .Select(parameter => new ParameterDescriptor
                        {
                            Name = parameter.Name,
                            ParameterType = parameter.ParameterType,
                            IsFromCap = parameter.GetCustomAttributes(typeof(FromPeppyAttribute)).Any()
                        }).ToList();

                    yield return InitDescriptor(attr, method, typeInfo, serviceTypeInfo, parameters);
                }
            }
        }

        private void RegisterMessageProcessor(ConsumerExecutorDescriptor descriptor)
        {
            _rabbitMqManager.OnMessageReceived += async (sender, transportMessage) =>
            {
                try
                {
                    _rabbitMqManager.CreateConnect(descriptor.Attribute.ExchangeName, descriptor.Attribute.QueueName);
                    var message = new Message(transportMessage.Headers, Encoding.UTF8.GetString(transportMessage.Body));
                    var value = message.Value.ToString().ToObject<Message>();
                    var consumerContext = new ConsumerContext(descriptor, value);
                    await Invoker.InvokeAsync(consumerContext, _cts.Token);
                    _rabbitMqManager.Commit(sender);
                    //_rabbitMQManager.Receive(descriptor.Attribute.ExchangeName, descriptor.Attribute.QueueName, (T) => Invoker.InvokeAsync(consumerContext, _cts.Token).GetAwaiter().GetResult());
                    //_rabbitMQManager.Commit(sender);
                }
                catch (Exception ex)
                {
                    _rabbitMqManager.Commit(sender);
                    //_rabbitMQManager.Reject(sender);
                }
            };
        }

        private ConsumerExecutorDescriptor InitDescriptor(
            AmqpAttribute attr,
            MethodInfo methodInfo,
            TypeInfo implType,
            TypeInfo serviceTypeInfo,
            IList<ParameterDescriptor> parameters)
        {
            var descriptor = new ConsumerExecutorDescriptor
            {
                Attribute = attr,
                MethodInfo = methodInfo,
                ImplTypeInfo = implType,
                ServiceTypeInfo = serviceTypeInfo,
                Parameters = parameters
            };

            return descriptor;
        }
    }
}