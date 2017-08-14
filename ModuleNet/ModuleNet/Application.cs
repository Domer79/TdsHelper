using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using ModuleNet.Abstractions;
using ModuleNet.Extensions;

namespace ModuleNet.ModuleNet
{
    public class Application
    {
        private Type _startupType;

        public void Run<TModule>(params object[] args) where TModule: class, IModule
        {
            DiContainer.Resolve<TModule>().Act(args);
        }

        public Application Init(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            foreach (var builderFunc in ConfigurationBuilderActions)
            {
                configurationBuilder.AddCustomBuilder(builderFunc);
            }
            Configuration = configurationBuilder.Build();

            DiContainer.Init(_startupType != null? (BaseStartup)Activator.CreateInstance(_startupType) : null);

            FindAndRegisterModules();

            return this;
        }

        public Application UseStartup<T>() where T: BaseStartup
        {
            _startupType = typeof(T);
            return this;
        }

        private void FindAndRegisterModules()
        {
            var moduleTypes = InternalService.Instance.GetAllTypes().Where(t => t.GetInterfaces().Contains(typeof(IModule)));
            foreach (var moduleType in moduleTypes)
            {
                DiContainer.AddTransient(moduleType);
            }
        }

        public static IConfigurationRoot Configuration { get; private set; }

        public static List<Func<IConfigurationBuilder, IConfigurationBuilder>> ConfigurationBuilderActions { get; } = new List<Func<IConfigurationBuilder, IConfigurationBuilder>>();
    }
}