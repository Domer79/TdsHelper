using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModuleNet.ModuleNet.Attributes;

namespace ModuleNet.ModuleNet
{
    class DiContainer: IServiceProvider
    {
        private readonly BaseStartup _startup;
        private readonly IServiceCollection _services = new ServiceCollection();
        private IServiceProvider _serviceProvider;
        private static DiContainer _container;

        private DiContainer(BaseStartup startup)
        {
            _startup = startup;
            ConfigureServices();

            _serviceProvider = _services.BuildServiceProvider();
        }

        private void ConfigureServices()
        {
            _services.AddOptions();
            _startup.Init(_services);

            LoadService();
        }

        private void LoadService()
        {
            var serviceTypeInfos = Enumerable.Where<Type>(InternalService.Instance.GetAllTypes(), t => IntrospectionExtensions.GetTypeInfo(t).IsDefined(typeof(InjectableAttribute)))
                .Select(t =>
                {
                    var injectableAttribute = t.GetTypeInfo().GetCustomAttribute<InjectableAttribute>();
                    injectableAttribute.ServiceType = t;
                    return injectableAttribute;
                });

            foreach (var serviceTypeInfo in serviceTypeInfos)
            {
                _services.Add(new ServiceDescriptor(serviceTypeInfo.ServiceType, serviceTypeInfo.ServiceType, (ServiceLifetime)serviceTypeInfo.ServiceLifetime));
            }
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public static void Init(BaseStartup startup)
        {
            _container = new DiContainer(startup);
        }

        public static T Resolve<T>()
        {
            return _container.GetService<T>();
        }

        public static IServiceProvider Provider => _container;

        internal static void AddSingleton<T>(T service) where T : class
        {
            _container._services.AddSingleton(service);
            _container._serviceProvider = _container._services.BuildServiceProvider();
        }

        internal static void AddTransient<T>() where T : class
        {
            AddTransient(typeof(T));
        }

        internal static void AddTransient(Type serviceType)
        {
            _container._services.AddTransient(serviceType);
            _container._serviceProvider = _container._services.BuildServiceProvider();
        }
    }
}
