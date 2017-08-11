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
        private readonly PostgresOptions _postgresOptions;

        public PgDbService(IOptions<PostgresOptions> postgresOptions)
        {
            _postgresOptions = postgresOptions.Value;
        }

        public bool CheckServerExist(string serverName)
        {
            using (var conn = new NpgsqlConnection(_postgresOptions.ConnectOptions.ToString()))
            {
                return conn.Query<bool>(Queries.PgCheckForeignServer, new {serverName}).Single();
            }
        }

        public bool UserIsMapped(string userName, string serverName)
        {
            using (var conn = new NpgsqlConnection(_postgresOptions.ConnectOptions.ToString()))
            {
                return conn.Query<bool>(Queries.PgCheckUserMapping, new {userName = userName.ToUpper(), serverName}).Single();
            }
        }

        public void ExecuteScript(string script)
        {
            using (var conn = new NpgsqlConnection(_postgresOptions.ConnectOptions.ToString()))
            {
                conn.Execute(script);
            }
        }
    }
}
