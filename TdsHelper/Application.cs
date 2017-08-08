using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TdsHelper.Models;

namespace TdsHelper
{
    class Application
    {
        private readonly MssqlConnectOptions _mssqlConnectOptions;
        private readonly PostgresTypeMapper _typeMapper;

        public Application(IOptions<MssqlConnectOptions> mssqlConnectOptions, PostgresTypeMapper typeMapper)
        {
            _mssqlConnectOptions = mssqlConnectOptions.Value;
            _typeMapper = typeMapper;
        }

        public static void Init(string[] args)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddCommandLine(args, new Dictionary<string, string>()
                {
                    { "-server", "mssql:server"},
                    { "-database", "mssql:database"},
                    { "-userid", "mssql:userid" },
                    { "-password", "mssql:password"},
                    { "-table", "mssql:table" }
                }).Build();

            DiContainer.Init();
        }

        public static IConfigurationRoot Configuration { get; private set; }

        public void Run()
        {
            using (IDbConnection conn = DiContainer.Resolve<IDbConnection>())
            {
                var columns = conn.Query<Column>(Queries.TableFullSchemaQuery, new { tablename = _mssqlConnectOptions.Table}).ToArray();
                var table = new Table()
                {
                    TableCatalog = columns.First().TableCatalog,
                    TableSchema = columns.First().TableSchema,
                    TableName = columns.First().TableName,
                    Columns = columns
                };

                var sb = new StringBuilder();
                sb.AppendLine($"create foreign table {table.TableName}");
                sb.AppendLine("(");

                foreach (var column in columns)
                {
                    sb.AppendLine(_typeMapper.ToPostgresColumnString(column));
                }

                sb.AppendLine(")");
                sb.AppendLine($"server {_mssqlConnectOptions.Server}");
                sb.AppendLine($"options (schema_name '{table.TableSchema}', table_name '{table.TableName}', row_estimate_method 'showplan_all')");
            }
        }
    }
}