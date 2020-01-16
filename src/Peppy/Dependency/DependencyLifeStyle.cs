using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Dependency
{
    /// <summary>
    /// Lifestyles of types used in dependency injection system.
    /// </summary>
    public enum DependencyLifeStyle
    {
        /// <summary>
        /// Scoped object. Created one object for only one instance in the same scope
        /// </summary>
        Scoped = 0,

        /// <summary>
        /// Singleton object. Created a single object on first resolving and same instance is used for subsequent resolves.
        /// </summary>
        Singleton = 1,

        /// <summary>
        /// Transient object. Created one object for every resolving.
        /// </summary>
        Transient = 2
    }
}