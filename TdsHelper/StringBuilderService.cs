using System;
using System.Text;
using Microsoft.Extensions.Options;
using ModuleNet.ModuleNet.Attributes;
using TdsHelper.Exceptions;
using TdsHelper.Models;
using TdsHelper.Services;

namespace TdsHelper
{
    [Injectable]
    public class StringBuilderService
    {
        private readonly PostgresTypeMapper _typeMapper;
        private readonly MssqlConnectOptions _msConnectOptions;
        private readonly PostgresOptions _pgOptions;
        private readonly PgDbService _pgDbService;

        public StringBuilderService(PostgresTypeMapper typeMapper, IOptions<MssqlConnectOptions> msConnectOptions,
            IOptions<PostgresOptions> pgOptions, PgDbService pgDbService)
        {
            _typeMapper = typeMapper;
            _msConnectOptions = msConnectOptions.Value;
            _pgOptions = pgOptions.Value;
            _pgDbService = pgDbService;
        }

        public StringBuilderService CreateTdsExtension(StringBuilder builder)
        {
            builder.AppendLine("create extension if not exists tds_fdw;")
                .AppendLine();
            return this;
        }

        public StringBuilderService CreateServerForDb(StringBuilder builder, Table table)
        {
            var serverExist = _pgDbService.CheckServerExist(_msConnectOptions.AliasAndServer);
            if (serverExist)
            {
                Console.WriteLine($"Server {_msConnectOptions.AliasAndServer} already exists. Operation will be skipped.");
                return this;
            }

            if (_pgOptions.ConnectOptions.ServerAddressIsIp() && string.IsNullOrEmpty(_msConnectOptions.MssqlServerAlias))
                throw new CreateScriptException("The server alias is not installed, and the connection to the server is indicated via its ip-address!");

            var serverAlias = _msConnectOptions.AliasAndServer;
            builder
                .AppendLine($"create server {serverAlias} foreign data wrapper tds_fdw options(servername '{_msConnectOptions.Server}', database '{_msConnectOptions.Database}', tds_version '7.1');")
                .AppendLine();

            return this;
        }

        public StringBuilderService CreateUserMapping(StringBuilder builder)
        {
            var userIsMapped = _pgDbService.UserIsMapped(_pgOptions.UserMap.PgAuthId, _msConnectOptions.AliasAndServer);
            if (userIsMapped)
            {
                Console.WriteLine($"Auth Id {_pgOptions.UserMap.PgAuthId} already mapped. Operation will be skipped.");
                return this;
            }

            builder
                .AppendLine(
                $"create user mapping for {_pgOptions.UserMap.PgAuthId} server {_msConnectOptions.AliasAndServer} options(username '{_pgOptions.UserMap.MapFor.UserName}', password '{_pgOptions.UserMap.MapFor.Password}');")
                .AppendLine();

            return this;
        }

        public StringBuilderService CreateSchema(StringBuilder builder)
        {
            builder.AppendLine($"create schema if not exists {_msConnectOptions.AliasAndServer}_public;")
                .AppendLine();

            return this;
        }

        public StringBuilderService DropTableIfExists(StringBuilder builder, Table table)
        {
            builder.AppendLine($"drop foreign table if exists {table.TableName} cascade;")
                .AppendLine();

            return this;
        }

        public StringBuilderService CreateTableScript(StringBuilder builder, Table table)
        {
            var columns = table.Columns;
            builder
                .AppendLine($"create foreign table if not exists {_msConnectOptions.AliasAndServer}_public.{table.TableName}")
                .AppendLine("(");

            for (var i = 0; i < columns.Length; i++)
            {
                builder.Append(_typeMapper.ToPostgresColumnString(columns[i]));

                if (i != columns.Length - 1)
                {
                    builder.AppendLine(",");
                    continue;
                }

                builder.AppendLine();
            }

            builder
                .AppendLine(")")
                .AppendLine($"server {_msConnectOptions.AliasAndServer}")
                .AppendLine(
                    $"options (schema_name '{table.TableSchema}', table_name '{table.TableName}', row_estimate_method 'showplan_all');");

            return this;
        }
    }
}
