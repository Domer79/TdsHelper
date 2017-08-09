namespace TdsHelper.Models
{
    public class PostgresConnectOptions
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"server={Server};database={Database};userid={UserId};password={Password};";
        }
    }
}