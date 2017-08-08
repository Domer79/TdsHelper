using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

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
            Application.Init(args);
            var application = DiContainer.Resolve<Application>();
            application.Run();
        }

        public static Dictionary<string, string> GetSwitchMappings(IReadOnlyDictionary<string, string> configurationStrings)
        {
            return configurationStrings.Select(item =>
                    new KeyValuePair<string, string>(
                        "-" + item.Key.Substring(item.Key.LastIndexOf(':') + 1),
                        item.Key))
                .ToDictionary(
                    item => item.Key, item => item.Value);
        }

        private static void ConfigurationTest(string[] args)
        {
            var dict = new Dictionary<string, string>
            {
                {"Profile:MachineName", "Rick"},
                {"App:MainWindow:Left", "11"}
            };

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            builder.AddInMemoryCollection(dict)
                .AddCommandLine(args, GetSwitchMappings(dict));
            Configuration = builder.Build();
            Console.WriteLine($"Hello {Configuration["Profile:MachineName"]}");

            // Set the default value to 80
            var left = Configuration.GetValue<int>("App:MainWindow:Left", 80);
            Console.WriteLine($"Left {left}");
            Console.WriteLine($"ConnectionString {Configuration["ConnectionStrings:DefaultConnection"]}");
            Console.ReadLine();
        }

        private static void ShowSchema(string tableName)
        {
            var query = @"select
                            TABLE_SCHEMA, 
                            ORDINAL_POSITION, 
                            COLUMN_NAME, 
                            DATA_TYPE, 
                            case DATA_TYPE
                                when ''varchar'' then COLUMN_NAME +'' varchar('' + cast(CHARACTER_MAXIMUM_LENGTH as nvarchar(10)) + ''),''

                            when ''nvarchar'' then COLUMN_NAME +'' varchar('' + cast(CHARACTER_MAXIMUM_LENGTH as nvarchar(10)) + ''),''

                            when ''uniqueidentifier'' then COLUMN_NAME +'' uuid,''
                            else COLUMN_NAME + '' '' + DATA_TYPE + '',''

                            end dataType,
                                CHARACTER_MAXIMUM_LENGTH
                            from
                                INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @table_name order by ordinal_position";

            using (var connection = new SqlConnection("server=172.23.2.65;user id=sa;password=123;database=testdb2;"))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"exec sp_executesql N'{query}', N'@table_name nvarchar(200)', @table_name = N'{tableName}'";

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0],10}| {reader[1],4}| {reader[2],10}| {reader[3],10}| {reader[4],20}| {reader[5],5}| ");
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static void TestSqlConnection()
        {
            using (var connection = new SqlConnection("server=172.23.2.65;user id=sa;password=123;database=testdb2;"))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Select id, firstName, lastName from table2";
                Console.WriteLine(command.CommandText);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"id: {reader["id"]:5}| firstName: {reader["firstName"]:45}| lastName: {reader["lastName"]:75}");
                        }
                    }

                }
                finally
                {
                    connection.Close();   
                }
            }
        }
    }
}