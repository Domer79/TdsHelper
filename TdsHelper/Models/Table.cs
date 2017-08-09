namespace TdsHelper.Models
{
    public class Table
    {
        public string TableCatalog { get; set; }
        public string TableSchema { set; get; }
        public string TableName { get; set; }
        public Column[] Columns { get; set; }
    }
}