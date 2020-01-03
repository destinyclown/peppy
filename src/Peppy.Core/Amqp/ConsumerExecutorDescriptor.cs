using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Peppy.Core.Amqp
{
    public class ConsumerExecutorDescriptor
    {
        public TypeInfo ServiceTypeInfo { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public TypeInfo ImplTypeInfo { get; set; }

        public AmqpAttribute Attribute { get; set; }

        public IList<ParameterDescriptor> Parameters { get; set; }
    }

    public class ParameterDescriptor
    {
        public string Name { get; set; }

        public Type ParameterType { get; set; }

        public bool IsFromCap { get; set; }
    }
}