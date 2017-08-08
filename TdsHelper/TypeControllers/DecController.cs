using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class DecController: BaseController
    {
        public DecController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"DEC({Column.NumericPrecision},{Column.NumericScale})";
        }
    }
}