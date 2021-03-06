﻿using System;
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
        private readonly ErrorLogger _errorLogger;
        private readonly TableOptions _tableOptions;
        private readonly MssqlConnectOptions _options;

        public MssqlDbService(IOptions<MssqlConnectOptions> options, IOptions<TableOptions> tableOptions, ErrorLogger errorLogger)
        {
            _errorLogger = errorLogger;
            _tableOptions = tableOptions.Value;
            _options = options.Value;
        }

        public Table GetTable(string tableName)
        {
            using (var conn = new SqlConnection(_options.ToString()))
            {
                try
                {
                    return conn.Query<Table>(Queries.MsTableSchema, new {tablename = tableName}).First();

                }
                catch (Exception e)
                {
                    _errorLogger.SaveErrorToLog(e.Message, e.StackTrace);
                    throw;
                }
            }
        }

        public Column[] GetTableColumns(string tableName)
        {
            using (IDbConnection conn = new SqlConnection(_options.ToString()))
            {
                try
                {
                    var columns = conn.Query<Column>(Queries.MsTableFullSchemaQuery, new {tablename = _tableOptions.Name})
                        .ToArray();

                    return columns;

                }
                catch (Exception e)
                {
                    _errorLogger.SaveErrorToLog(e.Message, e.StackTrace);
                    throw;
                }
            }
        }
    }
}
