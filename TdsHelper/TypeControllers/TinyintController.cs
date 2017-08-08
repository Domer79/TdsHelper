using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class TinyintController: BaseController
    {
        public TinyintController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"SMALLINT";
        }
    }
}