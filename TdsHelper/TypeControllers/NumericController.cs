using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class NumericController : BaseController
    {
        public NumericController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"Numeric({Column.NumericPrecision},{Column.NumericScale})";
        }
    }
}