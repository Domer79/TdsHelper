namespace TdsHelper.Models
{
    public class MssqlConnectOptions
    {
        public string MssqlServerAlias { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Table { get; set; }

        public string AliasAndServer => $"{MssqlServerAlias}_server";

        public override string ToString()
        {
            return $"server={Server};database={Database};user id={UserId};password={Password};";
        }
    }
}
