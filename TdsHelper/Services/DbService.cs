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
    class DbService
    {
        private readonly MssqlConnectOptions _options;

        public DbService(IOptions<MssqlConnectOptions> options)
        {
            _options = options.Value;
        }

        public Table GetTable(string tableName)
        {
            using (var conn = DiContainer.Resolve<IDbConnection>())
            {
                return conn.Query<Table>(Queries.TableSchema, new {tablename = tableName}).First();
            }
        }

        public Column[] GetTableColumns(string tableName)
        {
            using (IDbConnection conn = DiContainer.Resolve<IDbConnection>())
            {
                var columns = conn.Query<Column>(Queries.TableFullSchemaQuery, new {tablename = _options.Table})
                    .ToArray();

                return columns;
            }
        }
    }
}
