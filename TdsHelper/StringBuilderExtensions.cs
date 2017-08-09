using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using TdsHelper.Models;
using TdsHelper.Services;

namespace TdsHelper
{
    static class StringBuilderExtensions
    {
        private static readonly PostgresTypeMapper TypeMapper = DiContainer.Resolve<PostgresTypeMapper>();
        private static readonly MssqlConnectOptions MsConnectOptions = DiContainer.Resolve<IOptions<MssqlConnectOptions>>().Value;
        private static readonly PostgresOptions PgOptions = DiContainer.Resolve<IOptions<PostgresOptions>>().Value;
        private static readonly PgDbService PgDbService = DiContainer.Resolve<PgDbService>();

        public static StringBuilder CreateServerForDb(this StringBuilder builder, Table table)
        {
            var serverExist = PgDbService.CheckServerExist($"{PgOptions.MssqlServerAlias}_{MsConnectOptions.Database}_server");
            if (!serverExist)


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
            builder.AppendLine($"server {MsConnectOptions.Server}");
            builder.AppendLine(
                $"options (schema_name '{table.TableSchema}', table_name '{table.TableName}', row_estimate_method 'showplan_all')");

            return builder;
        }
    }
}
