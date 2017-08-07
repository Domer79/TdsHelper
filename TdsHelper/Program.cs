using System;
using System.Data;
using System.Data.SqlClient;

namespace TdsHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Show schema for table1");
            ShowSchema("table1");

            

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
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