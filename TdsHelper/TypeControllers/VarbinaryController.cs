using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class VarbinaryController: BaseController
    {
        public VarbinaryController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"BYTEA";
        }
    }
}