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
using TdsHelper.Services;

namespace TdsHelper
{
    class Application
    {
        private readonly MssqlConnectOptions _mssqlConnectOptions;
        private readonly PostgresTypeMapper _typeMapper;
        private readonly DbService _dbService;

        public Application(IOptions<MssqlConnectOptions> mssqlConnectOptions, PostgresTypeMapper typeMapper, DbService dbService)
        {
            _mssqlConnectOptions = mssqlConnectOptions.Value;
            _typeMapper = typeMapper;
            _dbService = dbService;
        }

        public void Run()
        {
            var table = _dbService.GetTable(_mssqlConnectOptions.Table);
            table.Columns = _dbService.GetTableColumns(_mssqlConnectOptions.Table);

            var sb = new StringBuilder().CreateServerForDb().CreateTableScript(table);
            var script = sb.ToString();
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


    }
}