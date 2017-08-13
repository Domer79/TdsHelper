using Microsoft.Extensions.DependencyInjection;

namespace ModuleNet.ModuleNet
{
    public abstract class BaseStartup
    {
        internal void Init(IServiceCollection services)
        {
            services.AddOptions();
            Configure(services);
        }

        protected abstract void Configure(IServiceCollection services);
    }
}