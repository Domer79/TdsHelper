namespace TdsHelper.Models
{
    public class TableOptions
    {
        public string Name { get; set; }
        public string[] Columns { get; set; }
        public string[] ExcludedColumns { get; set; }

        public ColumnsBuildPolicy ColumnsBuildPolicy
        {
            get
            {
                if (Columns == null || Columns.Length == 0)
                    if(ExcludedColumns == null || ExcludedColumns.Length == 0)
                        return ColumnsBuildPolicy.All;
                    else
                        return ColumnsBuildPolicy.AllColumnsExceptExcluded;

                return ColumnsBuildPolicy.OnlySpecifiedColumns;
            }
        }
    }

    public enum ColumnsBuildPolicy
    {
        All,
        OnlySpecifiedColumns,
        AllColumnsExceptExcluded,
    }
}