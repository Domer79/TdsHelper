using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class SmallmoneyController: BaseController
    {
        public SmallmoneyController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return "MONEY";
        }
    }
}