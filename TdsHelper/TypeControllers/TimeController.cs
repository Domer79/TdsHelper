using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class TimeController: BaseController
    {
        public TimeController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"TIME({(Column.DatetimePrecision > 6 ? 6 : Column.DatetimePrecision)})";
        }
    }
}