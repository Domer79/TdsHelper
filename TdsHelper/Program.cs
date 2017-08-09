using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TdsHelper.Modules;

namespace TdsHelper
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args = null)
        {
            ApplicationRun(args);
        }

        private static void ApplicationRun(string[] args)
        {
            Application.ConfigurationBuilderActions.Add((builder) => builder.AddCommandLine(args, new Dictionary<string, string>()
            {
                {"-ms-server", "mssql:server"},
                {"-ms-database", "mssql:database"},
                {"-ms-userid", "mssql:userid"},
                {"-ms-password", "mssql:password"},
                {"-ms-table", "mssql:table"},
            }));

            var app = new Application()
            .Init(args);

            app.Run<GenerateScriptModule>();
        }
    }
}