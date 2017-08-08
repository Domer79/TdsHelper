using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class NvarcharController: BaseController
    {
        public NvarcharController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            if (Column.CharacterMaximumLength == -1)
                return "TEXT";

            return $"VARCHAR({Column.CharacterMaximumLength})";
        }
    }
}