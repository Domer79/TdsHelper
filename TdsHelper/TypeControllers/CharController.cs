using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class CharController: BaseController
    {
        public CharController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"CHAR({Column.CharacterMaximumLength})";
        }
    }
}