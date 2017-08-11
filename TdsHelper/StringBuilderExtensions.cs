using System;
using System.Text;
using Microsoft.Extensions.Options;
using TdsHelper.Exceptions;
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

        public static StringBuilder CreateTdsExtension(this StringBuilder builder)
        {
            builder.AppendLine("create extension if not exists tds_fdw;")
                .AppendLine();
            return builder;
        }

        public static StringBuilder CreateServerForDb(this StringBuilder builder, Table table)
        {
            var serverExist = PgDbService.CheckServerExist(MsConnectOptions.AliasAndServer);
            if (serverExist)
            {
                Console.WriteLine($"Server {MsConnectOptions.AliasAndServer} already exists. Operation will be skipped.");
                return builder;
            }

            if (PgOptions.ConnectOptions.ServerAddressIsIp() && string.IsNullOrEmpty(MsConnectOptions.MssqlServerAlias))
                throw new CreateScriptException("Алиас сервера не установлен, а подключение к серверу указано через его ip-адрес!");

            var serverAlias = MsConnectOptions.AliasAndServer;
            return builder
                .AppendLine($"create server {serverAlias} foreign data wrapper tds_fdw options(servername '{MsConnectOptions.Server}', database '{MsConnectOptions.Database}', tds_version '7.1');")
                .AppendLine();
        }

        public static StringBuilder CreateUserMapping(this StringBuilder builder)
        {
            var userIsMapped = PgDbService.UserIsMapped(PgOptions.UserMap.PgAuthId, MsConnectOptions.AliasAndServer);
            if (userIsMapped)
            {
                Console.WriteLine($"Auth Id {PgOptions.UserMap.PgAuthId} already mapped. Operation will be skipped.");
                return builder;
            }

            return builder
                .AppendLine(
                $"create user mapping for {PgOptions.UserMap.PgAuthId} server {MsConnectOptions.AliasAndServer} options(username '{PgOptions.UserMap.MapFor.UserName}', password '{PgOptions.UserMap.MapFor.Password}');")
                .AppendLine();
        }

        public static StringBuilder CreateSchema(this StringBuilder builder)
        {
            return builder.AppendLine($"create schema if not exists {MsConnectOptions.AliasAndServer}_public;")
                .AppendLine();
        }

        public static StringBuilder DropTableIfExists(this StringBuilder builder, Table table)
        {
            return builder.AppendLine($"drop foreign table if exists {table.TableName} cascade;")
                .AppendLine();
        }

        public static StringBuilder CreateTableScript(this StringBuilder builder, Table table)
        {
            var columns = table.Columns;
            builder
                .AppendLine($"create foreign table if not exists {MsConnectOptions.AliasAndServer}_public.{table.TableName}")
                .AppendLine("(");

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

            builder
                .AppendLine(")")
                .AppendLine($"server {MsConnectOptions.AliasAndServer}")
                .AppendLine(
                    $"options (schema_name '{table.TableSchema}', table_name '{table.TableName}', row_estimate_method 'showplan_all');");

            return builder;
        }
    }
}
