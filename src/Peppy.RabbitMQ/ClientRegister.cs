using Microsoft.Extensions.DependencyInjection;
using Peppy.Core.Amqp;
using Peppy.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static Peppy.RabbitMQReceiveAttribute;

namespace Peppy.RabbitMQ
{
    public class ClientRegister
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMQManager _rabbitMQManager;

        public ClientRegister(
            IServiceProvider serviceProvider,
            IRabbitMQManager rabbitMQManager)
        {
            _serviceProvider = serviceProvider;
            _rabbitMQManager = rabbitMQManager;
            Start();
        }

        public void Start()
        {
            var groupingMatches = FindFromControllerTypes();
            foreach (var matchGroup in groupingMatches)
            {
                RegisterMessageProcessor(matchGroup.MethodInfo);
                _rabbitMQManager.Listening(matchGroup.Attribute.ExchangeName, matchGroup.Attribute.QueueName);
            }
        }

        private IEnumerable<ConsumerExecutorDescriptor> FindFromControllerTypes()
        {
            var executorDescriptorList = new List<ConsumerExecutorDescriptor>();
            var types = Assembly.GetEntryAssembly().ExportedTypes;
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

        private void RegisterMessageProcessor(MethodInfo methodInfo)
        {
            _rabbitMQManager.OnMessageReceived += (sender, transportMessage) =>
            {
                try
                {
                    var ass = Assembly.LoadFile(methodInfo.Module.FullyQualifiedName);
                    object[] parametors = new object[] { Convert.ToBase64String(transportMessage.Body) };
                    var type = methodInfo.DeclaringType;
                    var obj = _serviceProvider.GetRequiredService(type);
                    methodInfo.Invoke(obj, parametors);
                    _rabbitMQManager.Commit(sender);
                }
                catch (Exception ex)
                {
                    _rabbitMQManager.Commit(sender);
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