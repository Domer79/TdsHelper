using System;
using System.Collections.Generic;
using System.Text;
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

        public GenerateScriptModule(IOptions<MssqlConnectOptions> mssqlConnectOptions, MssqlDbService mssqlDbService)
        {
            _mssqlConnectOptions = mssqlConnectOptions.Value;
            _mssqlDbService = mssqlDbService;
        }

        public void Act(params object[] args)
        {
            var table = _mssqlDbService.GetTable(_mssqlConnectOptions.Table);
            table.Columns = _mssqlDbService.GetTableColumns(_mssqlConnectOptions.Table);

            var sb = new StringBuilder().CreateServerForDb(table).CreateTableScript(table);
            var script = sb.ToString();
        }
    }
}
