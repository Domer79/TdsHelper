using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Npgsql;

namespace TdsHelper.Services
{
    public class DbConnectionService
    {
        public SqlConnection GetSqlConnection()
        {
            return (SqlConnection) DiContainer.Resolve<IDbConnection>();
        }

        public NpgsqlConnection GetPgConnection()
        {
            return DiContainer.Resolve<NpgsqlConnection>();
        }
    }
}
