using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using TdsHelper.Models;

namespace TdsHelper
{
    static class StringBuilderExtensions
    {
        private static readonly PostgresTypeMapper TypeMapper = DiContainer.Resolve<PostgresTypeMapper>();
        private static readonly MssqlConnectOptions ConnectOptions = DiContainer.Resolve<IOptions<MssqlConnectOptions>>().Value;

        public static StringBuilder CreateServerForDb(this StringBuilder builder)
        {
            return builder;
        }

        public static StringBuilder CreateTableScript(this StringBuilder builder, Table table)
        {
            var columns = table.Columns;
            builder.AppendLine($"create foreign table {table.TableName}");
            builder.AppendLine("(");

            for (var i = 0; i < columns.Length; i++)
            {
                builder.Append(TypeMapper.ToPostgresColumnString(columns[i]));

                if (i != columns.Length - 1)
                {
                    builder.AppendLine(",");
                    continue;
                }

                builder.AppendLine();
            }

            builder.AppendLine(")");
            builder.AppendLine($"server {ConnectOptions.Server}");
            builder.AppendLine(
                $"options (schema_name '{table.TableSchema}', table_name '{table.TableName}', row_estimate_method 'showplan_all')");

            return builder;
        }
    }
}
