namespace TdsHelper.Models
{
    public class PostgresOptions
    {
        public PostgresConnectOptions ConnectOptions { get; set; }
        public UserMapOptions UserMap { get; set; }
    }
}