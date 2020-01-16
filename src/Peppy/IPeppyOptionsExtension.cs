using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy
{
    /// <summary>
    /// options extension
    /// </summary>
    public interface IPeppyOptionsExtension
    {
        /// <summary>
        /// Registered child service.
        /// </summary>
        /// <param name="services"></param>
        void AddServices(IServiceCollection services);
    }
}