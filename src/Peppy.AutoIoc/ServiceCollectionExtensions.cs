using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Peppy.AutoIoc
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add auto ioc services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseType"></param>
        /// <param name="lifeCycle"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoIoc(this IServiceCollection services, Type baseType, LifeCycle lifeCycle = LifeCycle.Scoped)
        {
            if (!baseType.IsInterface)
            {
                throw new TypeLoadException("The status code must be an enumerated type");
            }

            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = System.IO.Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            var types = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
            var implementTypes = types.Where(x => x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                    switch (lifeCycle)
                    {
                        case LifeCycle.Singleton:
                            services.AddSingleton(interfaceType, implementType);
                            break;

                        case LifeCycle.Transient:
                            services.AddTransient(interfaceType, implementType);
                            break;

                        case LifeCycle.Scoped:
                            services.AddScoped(interfaceType, implementType);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(lifeCycle), lifeCycle, null);
                    }
            }
            return services;
        }
    }
}