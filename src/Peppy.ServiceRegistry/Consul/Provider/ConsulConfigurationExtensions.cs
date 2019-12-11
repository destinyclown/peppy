using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Peppy.ServiceRegistry.Consul.Provider
{
    public static class ConsulConfigurationExtensions
    {
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder configurationBuilder, IEnumerable<Uri> consulUrls, List<string> consulPaths)
        {
            foreach (var consulPath in consulPaths)
            {
                configurationBuilder.Add(new ConsulConfigurationSource(consulUrls, consulPath));
            }
            return configurationBuilder;
        }
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder configurationBuilder, IEnumerable<string> consulUrls, List<string> consulPaths)
        {
            return configurationBuilder.AddConsul(consulUrls.Select(u => new Uri(u)), consulPaths);
        }
    }
}
