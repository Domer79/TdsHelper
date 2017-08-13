using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Options;
using ModuleNet.ModuleNet.Attributes;
using TdsHelper.Models;

namespace TdsHelper.Services
{
    [Injectable]
    public class MssqlDbService
    {
        private readonly MssqlConnectOptions _options;

        public MssqlDbService(IOptions<MssqlConnectOptions> options)
        {
            _options = options.Value;
        }

        public Table GetTable(string tableName)
        {
            using (var conn = new SqlConnection(_options.ToString()))
            {
                return conn.Query<Table>(Queries.MsTableSchema, new {tablename = tableName}).First();
            }
        }

        public Column[] GetTableColumns(string tableName)
        {
            using (IDbConnection conn = new SqlConnection(_options.ToString()))
            {
                var columns = conn.Query<Column>(Queries.MsTableFullSchemaQuery, new {tablename = _options.Table})
                    .ToArray();

                return columns;
            }
        }
    }
}
