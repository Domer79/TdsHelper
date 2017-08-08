using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class RowversionController: BaseController
    {
        public RowversionController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return "BYTEA";
        }
    }
}