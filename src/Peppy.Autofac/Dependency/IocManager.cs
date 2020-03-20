using Microsoft.Extensions.DependencyInjection;
using Peppy.Dependency;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Peppy.Autofac.Dependency
{
    public class IocManager : IIocManager
    {
        private readonly IServiceCollection _services;

        public IocManager(IServiceCollection services)
        {
            _services = services;
        }

        void IIocRegistrar.Register<TService>()
        {
            _services.AddTransient<TService>();
        }

        void IIocRegistrar.Register<TService>(DependencyLifeStyle lifeStyle)
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Scoped:
                    _services.AddScoped<TService>();
                    break;

                case DependencyLifeStyle.Singleton:
                    _services.AddSingleton<TService>();
                    break;

                case DependencyLifeStyle.Transient:
                    _services.AddTransient<TService>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(lifeStyle), lifeStyle, null);
            }
        }

        public void RegisterAssembly(Assembly assembly)
        {
            var length = AutofacOptionsManager.Assemblies?.Length ?? 0;
            if (AutofacOptionsManager.Assemblies != null) AutofacOptionsManager.Assemblies[length] = assembly;
        }

        void IIocRegistrar.Register<TService, TImplementation>()
        {
            _services.AddTransient<TService, TImplementation>();
        }

        void IIocRegistrar.Register<TService, TImplementation>(DependencyLifeStyle lifeStyle)
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Scoped:
                    _services.AddScoped<TService, TImplementation>();
                    break;

                case DependencyLifeStyle.Singleton:
                    _services.AddSingleton<TService, TImplementation>();
                    break;

                case DependencyLifeStyle.Transient:
                    _services.AddTransient<TService, TImplementation>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(lifeStyle), lifeStyle, null);
            }
        }
    }
}