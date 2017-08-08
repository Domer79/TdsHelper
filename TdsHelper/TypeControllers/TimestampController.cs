using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class TimestampController: BaseController
    {
        public TimestampController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return "BYTEA";
        }
    }
}