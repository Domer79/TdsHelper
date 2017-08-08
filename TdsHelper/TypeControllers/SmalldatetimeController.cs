using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class SmalldatetimeController: BaseController
    {
        public SmalldatetimeController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"TIMESTAMP(0)";
        }
    }
}