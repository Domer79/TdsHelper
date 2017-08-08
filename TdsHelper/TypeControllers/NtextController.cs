using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class NtextController: BaseController
    {
        public NtextController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"TEXT({Column.CharacterMaximumLength})";
        }
    }
}