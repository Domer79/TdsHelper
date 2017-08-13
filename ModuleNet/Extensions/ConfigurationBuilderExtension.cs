using System;
using Microsoft.Extensions.Configuration;

namespace ModuleNet.Extensions
{
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddCustomBuilder(this IConfigurationBuilder builder, Func<IConfigurationBuilder, IConfigurationBuilder> getConfigSource)
        {
            return getConfigSource(builder);
        }
    }
}
