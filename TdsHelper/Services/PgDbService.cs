using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using TdsHelper.Models;

namespace TdsHelper.Services
{
    public class PgDbService
    {
        private readonly PostgresConnectOptions _connectOptions;

        public PgDbService(IOptions<PostgresOptions> postgresOptions)
        {
            _connectOptions = postgresOptions.Value.ConnectOptions;
        }

        public bool CheckServerExist(string serverName)
        {
            using (var conn = new NpgsqlConnection(_connectOptions.ToString()))
            {
                return conn.Query<bool>(Queries.PgCheckForeignServer, new {serverName}).Single();
            }
        }

        public bool CreateForeignServer()
        {
            using (var conn = new NpgsqlConnection(_connectOptions.ToString()))
            {
                conn.Execute(
                    $"create server {serverName} FOREIGN DATA WRAPPER tds_fdw options(servername '172.23.2.65', database 'testdb2', tds_version '7.1')");
            }
        }
    }
}
