using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TdsHelper.Abstractions;
using TdsHelper.Models;
using TdsHelper.Services;

namespace TdsHelper.Modules
{
    public class GenerateScriptModule: IModule
    {
        private readonly MssqlConnectOptions _mssqlConnectOptions;
        private readonly MssqlDbService _mssqlDbService;
        private readonly PgDbService _pgDbService;

        public GenerateScriptModule(IOptions<MssqlConnectOptions> mssqlConnectOptions, 
            MssqlDbService mssqlDbService, PgDbService pgDbService)
        {
            _mssqlConnectOptions = mssqlConnectOptions.Value;
            _mssqlDbService = mssqlDbService;
            _pgDbService = pgDbService;
        }

        public void Act(params object[] args)
        {
            var table = _mssqlDbService.GetTable(_mssqlConnectOptions.Table);
            table.Columns = _mssqlDbService.GetTableColumns(_mssqlConnectOptions.Table);

            var sb = new StringBuilder()
                .CreateTdsExtension()
                .CreateServerForDb(table)
                .CreateUserMapping()
                .CreateSchema()
                .DropTableIfExists(table)
                .CreateTableScript(table);
            var script = sb.ToString();

            if (Application.Configuration.GetValue<bool>("scriptshowonly"))
            {
                Console.WriteLine(script);
                Console.ReadKey();
                return;
            }

            _pgDbService.ExecuteScript(script);
        }
    }
}
