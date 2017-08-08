using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TdsHelper.Models;
using TdsHelper.Services;

namespace TdsHelper
{
    class DiContainer: IServiceProvider
    {
        private readonly IServiceCollection _services = new ServiceCollection();
        private IServiceProvider _serviceProvider;
        private static DiContainer _container;

        private DiContainer()
        {
            ConfigureServices();

            _serviceProvider = _services.BuildServiceProvider();
        }

        private void ConfigureServices()
        {
            _services.AddOptions();
            _services.AddSingleton<ControllerCollection>();
            _services.AddSingleton<ControllerFactory>();
            _services.AddSingleton<PostgresTypeMapper>();
            _services.Configure<MssqlConnectOptions>(Application.Configuration.GetSection("mssql"));
            _services.AddTransient<Application>();
            _services.AddTransient<IDbConnection>((provider) =>
            {
                var connectOptions = Provider.GetService<IOptions<MssqlConnectOptions>>();
                var connectionString = $"server={connectOptions.Value.Server};" +
                                       $"database={connectOptions.Value.Database};" +
                                       $"user id={connectOptions.Value.UserId};" +
                                       $"password={connectOptions.Value.Password};";
                return new SqlConnection(connectionString);
            });
            _services.AddSingleton<DbService>();
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public static void Init()
        {
            _container = new DiContainer();
        }

        public static T Resolve<T>()
        {
            return _container.GetService<T>();
        }

        public static IServiceProvider Provider => _container;

        public static void AddSingleton<T>(T service) where T : class
        {
            _container._services.AddSingleton(service);
            _container._serviceProvider = _container._services.BuildServiceProvider();
        }
    }
}
