namespace TdsHelper.Models
{
    public class PostgresOptions
    {
        public string MssqlServerAlias { get; set; }
        public PostgresConnectOptions ConnectOptions { get; set; }
    }
}