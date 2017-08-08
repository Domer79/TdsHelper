using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Options;
using TdsHelper.Models;

namespace TdsHelper
{
    public class MyDbConnection: IDbConnection
    {
        private readonly SqlConnection _connection;

        public MyDbConnection(IOptions<MssqlConnectOptions> options)
        {
            var connectionString = $"server={options.Value.Server};" +
                                   $"database={options.Value.Database}" +
                                   $"user id={options.Value.UserId}" +
                                   $"password={options.Value.Password}";

            _connection = new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public string ConnectionString { get; set; }
        public int ConnectionTimeout { get; }
        public string Database { get; }
        public ConnectionState State { get; }
    }
}
