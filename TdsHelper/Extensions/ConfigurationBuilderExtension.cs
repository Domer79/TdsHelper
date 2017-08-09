using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace TdsHelper.Extensions
{
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddCustomBuilder(this IConfigurationBuilder builder, Func<IConfigurationBuilder, IConfigurationBuilder> getConfigSource)
        {
            return getConfigSource(builder);
        }
    }
}
