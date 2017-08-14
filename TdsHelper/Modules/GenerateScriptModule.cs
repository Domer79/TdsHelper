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
using System.IO;

namespace TdsHelper.Modules
{
    public class GenerateScriptModule: IModule
    {
        private readonly MssqlDbService _mssqlDbService;
        private readonly PgDbService _pgDbService;
        private readonly StringBuilderService _stringBuilderService;
        private readonly MssqlConnectOptions _mssqlConnectOptions;
        private readonly TableOptions _tableOptions;

        public GenerateScriptModule(MssqlDbService mssqlDbService, PgDbService pgDbService, StringBuilderService stringBuilderService, IOptions<TableOptions> tableOptions, IOptions<MssqlConnectOptions> mssqlConnectOptions)
        {
            _mssqlDbService = mssqlDbService;
            _pgDbService = pgDbService;
            _stringBuilderService = stringBuilderService;
            _mssqlConnectOptions = mssqlConnectOptions.Value;
            _tableOptions = tableOptions.Value;
        }

        public void Act(params object[] args)
        {
            var table = _mssqlDbService.GetTable(_tableOptions.Name);
            table.Columns = _mssqlDbService.GetTableColumns(_tableOptions.Name);

            var sb = new StringBuilder();
            _stringBuilderService.CreateTdsExtension(sb)
                .CreateServerForDb(sb, table)
                .CreateUserMapping(sb)
                .CreateSchema(sb)
                .DropTableIfExists(sb, table)
                .CreateTableScript(sb, table);

            var script = sb.ToString();

            if (Application.Configuration.GetValue<string>("scriptsavedirectory") != null)
            {
                SaveToFile($"{Application.Configuration.GetValue<string>("scriptsavedirectory")}{_mssqlConnectOptions.Database}_{_tableOptions.Name}.sql", script);
            }

            if (Application.Configuration.GetValue<bool>("scriptshowonly"))
            {
                Console.WriteLine(script);
                Console.Write("Execute script?[Y]");
                var keyInfo = Console.ReadKey();
                Console.WriteLine();
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

        private static void SaveToFile(string path, string content)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(path, content);
        }
    }
}
