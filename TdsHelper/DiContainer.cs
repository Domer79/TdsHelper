using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
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
            _services.Configure<MssqlConnectOptions>(Application.Configuration.GetSection("mssql"));
            _services.Configure<PostgresOptions>(Application.Configuration.GetSection("postgres"));

            _services.AddSingleton<ControllerCollection>();
            _services.AddSingleton<ControllerFactory>();
            _services.AddSingleton<PostgresTypeMapper>();
            _services.AddTransient<IDbConnection>((provider) =>
            {
                var connectOptions = Provider.GetService<IOptions<MssqlConnectOptions>>();
                var connectionString = $"server={connectOptions.Value.Server};" +
                                       $"database={connectOptions.Value.Database};" +
                                       $"user id={connectOptions.Value.UserId};" +
                                       $"password={connectOptions.Value.Password};";
                return new SqlConnection(connectionString);
            });
            _services.AddTransient((provider) =>
            {
                var connectOptions = Provider.GetService<IOptions<PostgresOptions>>().Value.ConnectOptions;
                var connectionString = $"server={connectOptions.Server};" +
                                       $"database={connectOptions.Database};" +
                                       $"userid={connectOptions.UserId};" +
                                       $"password={connectOptions.Password};";
                return new NpgsqlConnection(connectionString);
            });
            _services.AddSingleton<MssqlDbService>();
            _services.AddSingleton<PgDbService>();
            _services.AddSingleton<DbConnectionService>();
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
