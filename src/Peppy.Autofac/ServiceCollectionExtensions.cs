using Autofac;
using Autofac.Extensions.DependencyInjection;
using Peppy.Dependency;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions method
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add autofac services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutofac(this IServiceCollection services, Assembly[] assemblies)
        {
            var baseType = typeof(IDependency);
            return services.AddAutofac(baseType, assemblies);
        }

        /// <summary>
        /// Add autofac services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseType"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutofac(this IServiceCollection services, Type baseType, Assembly[] assemblies)
        {
            if (baseType == null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            //var containerbuilder = new ContainerBuilder();
            //containerbuilder.RegisterAssemblyTypes(assemblies)
            //        .Where(x => baseType.IsAssignableFrom(x) && x != baseType)
            //        .AsImplementedInterfaces()
            //        .InstancePerLifetimeScope();
            //containerbuilder.Populate(services);
            //var ApplicationContainer = containerbuilder.Build();
            //var result = new AutofacServiceProvider(ApplicationContainer);

            return services;
        }
    }
}