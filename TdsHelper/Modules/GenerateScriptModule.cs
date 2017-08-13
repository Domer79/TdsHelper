using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModuleNet.Abstractions;
using ModuleNet.ModuleNet;
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
        private readonly StringBuilderService _stringBuilderService;

        public GenerateScriptModule(IOptions<MssqlConnectOptions> mssqlConnectOptions, 
            MssqlDbService mssqlDbService, PgDbService pgDbService, StringBuilderService stringBuilderService)
        {
            _mssqlConnectOptions = mssqlConnectOptions.Value;
            _mssqlDbService = mssqlDbService;
            _pgDbService = pgDbService;
            _stringBuilderService = stringBuilderService;
        }

        public void Act(params object[] args)
        {
            var table = _mssqlDbService.GetTable(_mssqlConnectOptions.Table);
            table.Columns = _mssqlDbService.GetTableColumns(_mssqlConnectOptions.Table);

            var sb = new StringBuilder();
            _stringBuilderService.CreateTdsExtension(sb)
                .CreateServerForDb(sb, table)
                .CreateUserMapping(sb)
                .CreateSchema(sb)
                .DropTableIfExists(sb, table)
                .CreateTableScript(sb, table);

            var script = sb.ToString();

            if (Application.Configuration.GetValue<bool>("scriptshowonly"))
            {
                Console.WriteLine(script);
                Console.WriteLine("Execute script?[Y]");
                var keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Y)
                {
                    _pgDbService.ExecuteScript(script);
                    Console.WriteLine("Script successfull executed.");
                }

                return;
            }

            _pgDbService.ExecuteScript(script);
            Console.WriteLine("Script successfull executed.");
        }
    }
}
