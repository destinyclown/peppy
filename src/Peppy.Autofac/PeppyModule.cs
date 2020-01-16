using Autofac;
using System;

namespace Peppy.Autofac
{
    /// <summary>
    ///
    /// </summary>
    public class PeppyModule : Module
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="assemblies"></param>
        public PeppyModule(Type baseType, System.Reflection.Assembly[] assemblies)
        {
            BaseType = baseType;
            Assemblies = assemblies;
        }

        private Type BaseType { get; }

        private System.Reflection.Assembly[] Assemblies { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assemblies)
                    .Where(x => BaseType.IsAssignableFrom(x) && x != BaseType)
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }
}