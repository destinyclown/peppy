using Peppy.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Modules
{
    /// <summary>
    /// This class must be implemented by all module definition classes.
    /// </summary>
    public abstract class ModuleBase
    {
        /// <summary>
        /// Creates a new ModuleBase object.
        /// </summary>
        protected ModuleBase()
        {
        }

        /// <summary>
        /// Gets a reference to the IOC manager.
        /// </summary>
        protected internal IIocManager IocManager { get; }
    }
}