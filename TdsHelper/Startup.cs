using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModuleNet.ModuleNet;
using TdsHelper.Models;

namespace TdsHelper
{
    public class Startup: BaseStartup
    {
        protected override void Configure(IServiceCollection services)
        {
            services.Configure<MssqlConnectOptions>(Application.Configuration.GetSection("mssql"));
            services.Configure<PostgresOptions>(Application.Configuration.GetSection("postgres"));
            services.Configure<UserMapOptions>(Application.Configuration.GetSection("usermap"));
            services.Configure<TableOptions>(Application.Configuration.GetSection("table"));
        }
    }
}
