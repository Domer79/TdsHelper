using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Options;
using TdsHelper.Models;

namespace TdsHelper.Services
{
    public class MssqlDbService
    {
        private readonly MssqlConnectOptions _options;

        public MssqlDbService(IOptions<MssqlConnectOptions> options)
        {
            _options = options.Value;
        }

        public Table GetTable(string tableName)
        {
            using (var conn = DiContainer.Resolve<IDbConnection>())
            {
                return conn.Query<Table>(Queries.MsTableSchema, new {tablename = tableName}).First();
            }
        }

        public Column[] GetTableColumns(string tableName)
        {
            using (IDbConnection conn = DiContainer.Resolve<IDbConnection>())
            {
                var columns = conn.Query<Column>(Queries.MsTableFullSchemaQuery, new {tablename = _options.Table})
                    .ToArray();

                return columns;
            }
        }
    }
}
