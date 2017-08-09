using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TdsHelper.Abstractions;
using TdsHelper.Extensions;
using TdsHelper.Models;
using TdsHelper.Services;

namespace TdsHelper
{
    public class Application
    {
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

            DiContainer.Init();

            FindAndRegisterModules();

            return this;
        }

        private void FindAndRegisterModules()
        {
            var moduleTypes = Assembly.GetEntryAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IModule)));
            foreach (var moduleType in moduleTypes)
            {
                DiContainer.AddTransient(moduleType);
            }
        }

        public static IConfigurationRoot Configuration { get; private set; }

        public static List<Func<IConfigurationBuilder, IConfigurationBuilder>> ConfigurationBuilderActions { get; } = new List<Func<IConfigurationBuilder, IConfigurationBuilder>>();
    }
}