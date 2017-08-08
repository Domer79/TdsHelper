using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class IntegerController: BaseController
    {
        public IntegerController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"INTEGER";
        }
    }
}