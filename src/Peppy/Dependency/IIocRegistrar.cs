using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Peppy.Dependency
{
    /// <summary>
    /// Define interface for classes those are used to register dependencies.
    /// </summary>
    public interface IIocRegistrar
    {
        /// <summary>
        /// Registers a service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        void Register<TService>()
            where TService : class;

        /// <summary>
        /// Registers a service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="lifeStyle"></param>
        void Register<TService>(DependencyLifeStyle lifeStyle)
             where TService : class;

        /// <summary>
        /// Registers a service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// Registers a service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="lifeStyle"></param>
        void Register<TService, TImplementation>(DependencyLifeStyle lifeStyle)
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// Registers services of given assembly by all conventional registrars.
        /// </summary>
        /// <param name="assembly"></param>
        void RegisterAssembly(Assembly assembly);
    }
}